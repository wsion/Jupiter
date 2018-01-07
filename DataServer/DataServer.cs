using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Jupiter.Utility;
using System.Net.Sockets;
using System.Net;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Jupiter.NetworkServices.DataServer
{
    class DataServer
    {
        private List<DataClient> clients = new List<DataClient>();
        private Object isExit = false;
        Thread sqlMonitorThread, workingThread;

        private static EventWaitHandle evntWaitHandle =
            new EventWaitHandle(false, EventResetMode.AutoReset);

        public static DataServer Instance = new DataServer();

        private DataServer()
        {
        }

        public void Start()
        {
            Log.Info("Data Server Started");

            sqlMonitorThread = new Thread(MonitorDatabase);
            sqlMonitorThread.IsBackground = false;
            sqlMonitorThread.Start();

            workingThread = new Thread(StartWorking);
            workingThread.IsBackground = false;
            workingThread.Start();
        }

        public void Stop()
        {
            lock (isExit)
            {
                isExit = true;
            }
            SqlDependency.Stop(App.DbConnection);
            workingThread.Abort();
            sqlMonitorThread.Abort();
            Log.Info("Data Server Stopped");
        }

        private void StartWorking()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, App.ServerPort);
            listener.Start();

            while (!(Boolean)isExit)
            {
                try
                {
                    evntWaitHandle.Reset();
                    listener.BeginAcceptTcpClient(AcceptTcpClientCallback, listener);
                    evntWaitHandle.WaitOne();
                }
                catch (Exception ex)
                {
                    Log.Error("Main Thread - Exception:{0},\r\nStackTrace:{1}",
                        ex.Message, ex.StackTrace);
                }
            }
        }

        private void AcceptTcpClientCallback(IAsyncResult ar)
        {
            try
            {
                evntWaitHandle.Set();

                TcpListener listener = ar.AsyncState as TcpListener;
                TcpClient client = listener.EndAcceptTcpClient(ar);

                Log.Info("Client Connected:{0}", client.Client.RemoteEndPoint);

                DataClient dataClient = new DataClient(client);
                dataClient.OnClientIdChanging += DataClient_OnClientIdChanging;
                dataClient.Authenticate();
            }
            catch (Exception ex)
            {
                Log.Error("AcceptTcpClientCallback - Exception:{0},\r\nStackTrace:{1}",
                    ex.Message, ex.StackTrace);
            }
        }

        private void DataClient_OnClientIdChanging(string id, DataClient newClient)
        {
            lock (clients)
            {
                var oldClient = clients.FirstOrDefault(c => { return c.ID == id; });

                if (oldClient != null)
                {
                    clients.Remove(oldClient);
                }

                clients.Add(newClient);

                clients.ForEach(c => { Log.Info("DataClient_OnClientIdChanging:{0}", c.Uid); });
            }
        }

        private void MonitorDatabase()
        {
            try
            {
                SqlDependency.Start(App.DbConnection);
                using (SqlConnection con = new SqlConnection(App.DbConnection))
                using (SqlCommand cmd = new SqlCommand(App.DependencyQuery, con))
                {
                    con.Open();
                    cmd.CommandType = System.Data.CommandType.Text;
                    var dependency = new SqlDependency(cmd);
                    dependency.OnChange += Dependency_OnChange;
                    cmd.ExecuteNonQuery();
                    Log.Info("MonitorDatabase started");
                }
            }
            catch (SqlException ex)
            {
                Log.Error("MonitorDatabase Thread - SqlException:{0},\r\nStackTrace:{1}",
                   ex.Message, ex.StackTrace);
                Thread.Sleep(1000);
                MonitorDatabase();
            }
            catch (Exception ex)
            {
                Log.Error("MonitorDatabase Thread - Exception:{0},\r\nStackTrace:{1}",
                    ex.Message, ex.StackTrace);
            }
        }

        private void Dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            Log.Info("DB changes detected - Type:{0},Source:{1},Info:{2}",
                e.Type, e.Source, e.Info);

            if (e.Info == SqlNotificationInfo.Insert)
            {
                Task t = new Task(SendRealTimeData);
                t.Start();
            }

            MonitorDatabase();
        }

        private void SendRealTimeData()
        {
            try
            {
                var token = Guid.NewGuid().ToString();
                SqlDataAccess sqlDa = new SqlDataAccess();
                sqlDa.GenerateConnection(App.DbConnection, true);


                List<KeyValuePair<string, string>> clientsWithData = new List<KeyValuePair<string, string>>();
                List<string> unhandledIds = new List<string>();

                sqlDa.ExecuteReader(
                    App.ProcessQuery,
                    new { token = token },
                    (record) =>
                    {
                        var clientId = record[0].ToString();

                        StringBuilder sb = new StringBuilder();
                        for (int i = 1; i < record.FieldCount; i++)
                        {
                            var col = System.Web.HttpUtility.UrlEncode(record[i].ToString());
                            sb.Append(col + (i == record.FieldCount - 1 ? "" : "|"));
                        }

                        var kv = new KeyValuePair<string, string>(clientId, sb.ToString());
                        clientsWithData.Add(kv);
                    }
               );



                lock (clients)
                {
                    foreach (var item in clientsWithData)
                    {
                        var client = clients.FirstOrDefault(c => { return c.ID == item.Key; });
                        if (client != null)
                        {
                            try
                            {
                                client.SendMessage(string.Format("$START${0}$END$", item.Value));
                            }
                            catch
                            {
                                if (unhandledIds.IndexOf(item.Key) < 0)
                                {
                                    unhandledIds.Add(item.Key);
                                }
                            }
                        }
                        else
                        {
                            if (unhandledIds.IndexOf(item.Key) < 0)
                            {
                                unhandledIds.Add(item.Key);
                            }
                        }
                    }
                }

                unhandledIds.ForEach(id =>
                {
                    sqlDa.Execute(
                        App.AfterProcessQuery,
                        new { token = token, clientId = id },
                        false
                   );
                });
            }
            catch (Exception ex)
            {
                Log.Error("SendRealTimeData - Exception:{0},\r\nStackTrace:{1}",
                    ex.Message, ex.StackTrace);
            }
        }
    }
}
