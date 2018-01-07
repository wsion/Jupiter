using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Jupiter.Utility;
using System.Threading.Tasks;
using System.Threading;

namespace Jupiter.NetworkServices.DataServer
{
    class DataClient
    {
        private string id;
        private Guid guid;
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

        public Guid Uid
        {
            get
            {
                return this.guid;
            }
        }

        public event DataClientIdChangeEventHandler OnClientIdChanging;

        public DataClient(TcpClient Client)
        {
            this.guid = Guid.NewGuid();
            this.tcpClient = Client;
            networkStream = Client.GetStream();
            this.networkStream.ReadTimeout = 60000;
            this.networkStream.WriteTimeout = 60000;
            var sslStream = new System.Net.Security.SslStream(networkStream);
        }

        public void SendMessage(string message)
        {
            Log.Info("Client-{0}-SendingMessage", this.guid);
            this.sendBytes = System.Text.Encoding.UTF8.GetBytes(message);
            networkStream.BeginWrite(this.sendBytes, 0, this.sendBytes.Length, sendCallBack, message);
            networkStream.Flush();
        }

        private void sendCallBack(IAsyncResult ar)
        {
            try
            {
                if (tcpClient.Connected)
                {
                    string message = ar.AsyncState as string;
                    networkStream.EndWrite(ar);
                    Log.Info("Client-{0}-Message-Sent:{1}", this.id, message);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Client-{2}- Send Message Callback - Exception:{0},\r\nStackTrace:{1}",
                    ex.Message, ex.StackTrace, this.id);
            }
        }

        public void Authenticate()
        {
            var bufferSize = tcpClient.ReceiveBufferSize;
            this.readBytes = new byte[bufferSize];
            networkStream.BeginRead(this.readBytes, 0, bufferSize, readCallBack, null);

            SendMessage("Hello");
        }

        private void readCallBack(IAsyncResult ar)
        {
            try
            {
                int count = networkStream.EndRead(ar);
                var str = System.Text.Encoding.UTF8.GetString(this.readBytes, 0, count);
                var strArr = str.Split('-');

                //Refuse illegal connection
                //Expected data format:
                //AuthToken-ClientId-Message
                if (strArr.Length != 3 || strArr[0] != App.AuthenticationToken)
                {
                    networkStream.Close();
                    tcpClient.Close();
                    return;
                }

                if (isFirstCall)
                {
                    this.id = strArr[1];
                    Log.Info("Client-{0}-Connected", this.id);
                    if (OnClientIdChanging != null)
                    {
                        OnClientIdChanging(this.id, this);
                    }
                    isFirstCall = false;
                }
                else
                {
                    Log.Info("Client-{0}-Message-Received:{1}", this.id, strArr[2]);
                    //insertIntoDb(DateTime.Now.ToString());
                }
                Log.Info("Client-{0}-Message-Received:", this.guid);

                //Keep reading message from client
                var bufferSize = this.tcpClient.ReceiveBufferSize;
                this.readBytes = new byte[bufferSize];
                this.networkStream.BeginRead(this.readBytes, 0, bufferSize, readCallBack, null);
            }
            catch (Exception ex)
            {
                Log.Error("Client-{2}-READCALLBACK Error:{0},\r\nStackTrace:{1}",
                    ex.Message, ex.StackTrace, this.id);
            }
        }

        /*
        private void insertIntoDb(string message)
        {
            var da = new Utility.DataAccess("mssql", App.DbConnection);
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("@msg", message));
            da.ExecuteNonQuery("Insert Into Syslog VALUES(@msg)", list);
        }*/
    }

    delegate void DataClientIdChangeEventHandler(string id, DataClient client);
}
