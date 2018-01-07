using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Jupiter.Utility;
using System.Collections.Generic;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Dev.NetworkUtility
{
    class Server
    {
        private static EventWaitHandle evntWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

        private static List<ClientWorking> clients = new List<ClientWorking>();

        public static void Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8888);
            listener.Start();
            Console.WriteLine("Server running ...");

            Thread sqlMonitorThread = new Thread(MonitorDatabase);
            sqlMonitorThread.IsBackground = false;
            sqlMonitorThread.Start();

            while (true)
            {
                try
                {
                    evntWaitHandle.Reset();
                    PrintThread("Main - While loop");
                    listener.BeginAcceptTcpClient(AcceptTcpClientCallback, listener);
                    evntWaitHandle.WaitOne();
                }
                catch (Exception ex)
                {
                    Jupiter.Utility.Log.Error("Main Thread - Exception:{0},\r\nStackTrace:{1}",
                        ex.Message, ex.StackTrace);
                }
            }
        }

        public static void PrintThread(string msg = "")
        {
            Console.WriteLine("\r\n{0} Thread ID: {1}", msg, Thread.CurrentThread.ManagedThreadId);
        }

        private static void AcceptTcpClientCallback(IAsyncResult ar)
        {
            try
            {
                PrintThread("AcceptTcpClientCallback:");
                evntWaitHandle.Set();

                TcpListener listener = ar.AsyncState as TcpListener;
                TcpClient client = listener.EndAcceptTcpClient(ar);

                Console.WriteLine("Incoming client:{0}", client.Client.RemoteEndPoint);

                ClientWorking cwObj = new ClientWorking(client);
                clients.Add(cwObj);

                cwObj.Authenticate();
                cwObj.SendHello();
            }
            catch (Exception ex)
            {
                Jupiter.Utility.Log.Error("Callback Thread - Exception:{0},\r\nStackTrace:{1}",
                    ex.Message, ex.StackTrace);
            }
        }

        private static void MonitorDatabase()
        {
            var conStr = "Data Source=.;Initial Catalog=Jupiter;Integrated Security=True";
            var sql = "select id from [dbo].[messages]";
            SqlDependency.Start(conStr);
            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                con.Open();
                cmd.CommandType = System.Data.CommandType.Text;
                var dependency = new SqlDependency(cmd);
                dependency.OnChange += Dependency_OnChange;
                cmd.ExecuteNonQuery();
            }
        }

        private static void Dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            PrintThread("Dependency Thread - ");
            Jupiter.Utility.Log.Info("DB changes detected.");
            Console.WriteLine("changes detected.");

            lock (clients)
            {
                if (clients.Count > 0)
                {
                    clients[0].SendMessage("DB changes detected");
                }
            }

            MonitorDatabase();
        }
    }


    class ClientWorking
    {
        private string id;
        private NetworkStream networkStream;
        private TcpClient tcpClient;
        private byte[] readBytes;
        private byte[] sendBytes;
        private bool isFirstCall = true;

        public string ID
        {
            get
            {
                return id;
            }
        }

        public ClientWorking(TcpClient Client)
        {
            this.tcpClient = Client;
            networkStream = Client.GetStream();
            var sslStream = new System.Net.Security.SslStream(networkStream);            
        }

        public void SendHello()
        {
            SendMessage("Connection established.");
        }

        public void SendMessage(string message)
        {
            this.sendBytes = System.Text.Encoding.UTF8.GetBytes(message);
            //networkStream.BeginWrite(this.sendBytes, 0, this.sendBytes.Length, sendCallBack, this);
            networkStream.BeginWrite(this.sendBytes, 0, this.sendBytes.Length, sendCallBack, null);
            networkStream.Flush();
        }

        private void sendCallBack(IAsyncResult ar)
        {
            Server.PrintThread(string.Format("Client {0} SendCallback - ", this.id));

            try
            {
                networkStream.EndWrite(ar);
            }
            catch (Exception ex)
            {
                Jupiter.Utility.Log.Error("ClientWorking Send Message Callback - Exception:{0},\r\nStackTrace:{1}",
                    ex.Message, ex.StackTrace);
            }
        }

        public void Authenticate()
        {
            var bufferSize = tcpClient.ReceiveBufferSize;
            this.readBytes = new byte[bufferSize];
            networkStream.BeginRead(this.readBytes, 0, bufferSize, readCallBack, null);
        }

        private void readCallBack(IAsyncResult ar)
        {
            try
            {
                Server.PrintThread(string.Format("Client {0} ReadCallback - ", this.id));

                int count = networkStream.EndRead(ar);
                var str = System.Text.Encoding.UTF8.GetString(this.readBytes, 0, count);
                if (isFirstCall)
                {
                    this.id = str;
                    Console.WriteLine("\r\nClient ID:{0}", this.id);
                    isFirstCall = false;
                }
                else
                {
                    Console.WriteLine("\r\nClient [{0}] Says:\r\n{1}", this.id, str);
                }

                //Keep reading message from client
                var bufferSize = this.tcpClient.ReceiveBufferSize;
                this.readBytes = new byte[bufferSize];
                this.networkStream.BeginRead(this.readBytes, 0, bufferSize, readCallBack, null);
            }
            catch (Exception ex)
            {
                Jupiter.Utility.Log.Error("ClientWorking Authenticate Callback - Exception:{0},\r\nStackTrace:{1}",
                    ex.Message, ex.StackTrace);
            }
        }
    }
}
