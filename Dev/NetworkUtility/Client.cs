using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Dev.NetworkUtility
{
    class Client
    {
        TcpClient client;
        NetworkStream networkStream;
        byte[] readBuffer;

        public void Start()
        {
            Console.WriteLine("Client running ...");
            Server.PrintThread("Client Main Thread - ");
            connectServer();

            //Keep connection alive
            Thread.Sleep(100);
            Timer timer = new Timer(keepAlive, null, 0, 5000);

            Console.ReadLine();
        }

        private void connectServer()
        {
            try
            {
                this.client = new TcpClient();
                this.client.Connect(IPAddress.Parse("127.0.0.1"), 8888);
                this.networkStream = client.GetStream();

                //Send authentication message
                sendMessage(Guid.NewGuid().ToString());

                //Keep reading message
                this.readBuffer = new byte[this.client.ReceiveBufferSize];
                networkStream.BeginRead(this.readBuffer, 0, this.client.ReceiveBufferSize, readCallback, null);
            }
            catch (Exception ex)
            {
                Jupiter.Utility.Log.Error("Sever reading error: \r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
                Console.WriteLine("Cannot establish conneciton.");
            }
        }

        private void keepAlive(object obj)
        {
            Console.WriteLine("Sending [Keep Alive].");
            Server.PrintThread("Client KeepAlive - ");

            //Send keep alive message
            try
            {
                sendMessage(string.Format("KEEP_ALIVE {0}", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")));
            }
            catch (Exception ex)
            {
                Console.WriteLine("connected:{0}", this.client.Connected);
                connectServer();
            }
        }

        private void sendMessage(string message)
        {
            Server.PrintThread("Client SendMessage - ");
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
            networkStream.Write(buffer, 0, buffer.Length);
            networkStream.Flush();
        }

        private void readCallback(IAsyncResult ar)
        {
            try
            {
                if (this.client.Connected)
                {
                    Server.PrintThread("Client ReadMessage - ");
                    int count = networkStream.EndRead(ar);
                    var str = System.Text.Encoding.UTF8.GetString(this.readBuffer, 0, count);

                    Console.WriteLine("Message received: {0}", str);

                    this.readBuffer = new byte[this.client.ReceiveBufferSize];
                    networkStream.BeginRead(this.readBuffer, 0, this.client.ReceiveBufferSize, readCallback, null);
                }
            }
            catch (Exception ex)
            {
                Jupiter.Utility.Log.Error("Sever reading error: \r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
                if (!this.client.Connected)
                {
                    Console.WriteLine("Connection lost.");
                }
            }
        }
    }
}
