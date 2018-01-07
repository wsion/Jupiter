using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Jupiter.Utility;
using System.Threading.Tasks;
using System.Web;
using System.Collections.Generic;

namespace Jupiter.NetworkServices.DataReceiver
{
    class DataClient
    {
        TcpClient client;
        NetworkStream networkStream;
        byte[] readBuffer;
        Timer timer;
        Object isExit = false;
        EventWaitHandle evntWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

        public static DataClient Instance = new DataClient();

        private DataClient()
        {

        }

        public void Start()
        {
            Log.Info("Client Started");
            connectServer();

            Thread.Sleep(100);
            timer = new Timer(keepAlive, null, 0, 5000);
        }

        public void Stop()
        {
            lock (isExit)
            {
                isExit = true;
            }
            timer.Dispose();
        }

        private void startWorking()
        {
        }

        private void connectServer()
        {
            try
            {
                if (!(Boolean)isExit)
                {
                    //evntWaitHandle.Reset();
                    this.client = new TcpClient();
                    this.client.Connect(IPAddress.Parse(App.ServerIp), App.ServerPort);
                    this.networkStream = client.GetStream();
                    this.networkStream.ReadTimeout = 60000;
                    this.networkStream.WriteTimeout = 60000;

                    //Send authentication message
                    sendMessage("Hello");

                    //Keep reading message
                    this.readBuffer = new byte[this.client.ReceiveBufferSize];
                    networkStream.BeginRead(this.readBuffer, 0,
                        this.client.ReceiveBufferSize, readCallback, null);
                    networkStream.Flush();
                }

            }
            catch (Exception ex)
            {
                Log.Error("Reading message error: \r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
                Log.Error("Cannot establish conneciton.");
            }
        }

        private void keepAlive(object obj)
        {
            Log.Info("Sending [Keep Alive].");

            //Send keep alive message
            try
            {
                sendMessage("KEEP_CONNECTION_ALIVE");
            }
            catch (Exception ex)
            {
                Log.Info("connected:{0}", this.client.Connected);
                Log.Error("Sending message error: \r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
                connectServer();
            }
        }

        private void sendMessage(string message)
        {
            message = string.Format("{0}-{1}-{2}",
                App.AuthenticationToken,
                App.AuthenticationIdentity,
                message);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
            networkStream.Write(buffer, 0, buffer.Length);
            networkStream.Flush();
        }

        private void readCallback(IAsyncResult ar)
        {
            try
            {
                if (this.client.Connected && !(Boolean)isExit)
                {
                    //evntWaitHandle.Set();
                    int count = networkStream.EndRead(ar);
                    var str = System.Text.Encoding.UTF8.GetString(this.readBuffer, 0, count);

                    if (str.StartsWith("$START$"))
                    {
                        Log.Info("Data received: {0}", str);

                        var strArr = str.Split(new[] { "$END$" }, StringSplitOptions.None);
                        foreach (var item in strArr)
                        {
                            if (item.StartsWith("$START$"))
                            {
                                new Task(processData, item.Remove(0, 7)).Start();
                            }
                        }
                    }else
                    {
                        Log.Info("Message received: {0}", str);
                    }

                    this.readBuffer = new byte[this.client.ReceiveBufferSize];
                    networkStream.BeginRead(this.readBuffer, 0, this.client.ReceiveBufferSize, readCallback, null);
                    networkStream.Flush();
                    //evntWaitHandle.WaitOne();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Reading message error: \r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
                if (!this.client.Connected)
                {
                    Log.Info("Connection lost.");
                }
            }
        }

        private void processData(object state)
        {
            string data = state as string;
            try
            {
                string[] cols = data.Split('|');
                List<KeyValuePair<string, string>> parameters =
                    new List<KeyValuePair<string, string>>();
                for (int i = 0; i < cols.Length; i++)
                {
                    var key = App.DbParameterPrifix + i;
                    var value = HttpUtility.UrlDecode(cols[i]);
                    var para = new KeyValuePair<string, string>(key, value);
                    parameters.Add(para);
                }

                DataAccess da = new DataAccess(App.DbType, App.DbConnection);
                int rowsAffected = da.ExecuteNonQuery(App.ProcessDataQuery, parameters);
                Log.Info("Process Data, {0} rows affected.", rowsAffected);
            }
            catch (Exception ex)
            {
                Log.Error("ProcessData error: \r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
            }
        }
    }
}