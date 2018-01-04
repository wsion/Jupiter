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
        object stateObj = new object();

        public void Start()
        {
            Console.WriteLine("Client running ...");
            initializeTcpClient();
            keepAlive();
        }

        public void keepAlive()
        {
            Timer timer = new Timer(Timer_Elapsed, null, 0, 1000);
        }

        private void Timer_Elapsed(object obj)
        {
            Console.WriteLine("Timer Elapses");
            sendData();
        }

        private void initializeTcpClient()
        {
            //if (client == null || !client.Connected)
            //{
            try
            {
                client = new TcpClient();
                //client.Connect(IPAddress.Parse("118.190.117.0"), 8888);
                client.Connect(IPAddress.Parse("127.0.0.1"), 8888);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connected:{0}", client.Connected);
                Console.WriteLine(ex.Message);
            }
            //}
        }

        private void sendData()
        {
            try
            {
                lock (stateObj)
                {
                    string str = System.DateTime.Now.ToString();
                    var networkStream = client.GetStream();
                    byte[] buffer = Encoding.UTF8.GetBytes(str);
                    networkStream.Write(buffer, 0, buffer.Length);
                    Console.WriteLine("SendBufferSize:{0},SendTimeout:{1}\nReceiveBufferSize:{2},ReceiveTimeout:{3}",
                        client.SendBufferSize, client.SendTimeout,
                        client.ReceiveBufferSize, client.ReceiveTimeout);

                    int bytes = networkStream.Read(buffer, 0, buffer.Length);
                    if (bytes > 0)
                    {
                        str = Encoding.UTF8.GetString(buffer, 0, bytes);
                        if (str != "SUCESS")
                        {
                            initializeTcpClient();
                        }
                    }
                    else
                    {
                        initializeTcpClient();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connected:{0}", client.Connected);
                Console.WriteLine(ex.Message);
                initializeTcpClient();
            }
        }
    }
}
