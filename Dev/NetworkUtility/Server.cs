using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Dev.NetworkUtility
{
    class Server
    {
        public static void Start()
        {
            TcpListener server = new TcpListener(IPAddress.Any, 8888);
            server.Start();
            Console.WriteLine("Server running ...");
            while (true)
            {
                try
                {
                    ClientWorking cw = new ClientWorking(server.AcceptTcpClient());
                    new Thread(new ThreadStart(cw.ProcessData)).Start();
                    //cw.ProcessData();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error:{0}", ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Jupiter.Utility.Log.Error(ex.Message);
                    Jupiter.Utility.Log.Error(ex.StackTrace);
                }
            }
            server.Stop();
        }
    }

    class ClientWorking
    {
        private NetworkStream networkStream;
        private TcpClient Client;
        private IPEndPoint endPoint;

        public ClientWorking(TcpClient Client)
        {
            this.Client = Client;
            this.endPoint = (IPEndPoint)Client.Client.RemoteEndPoint;
            networkStream = Client.GetStream();
        }

        public void ProcessData()
        {
            string data;
            while (Client.Connected && networkStream.CanRead)
            {
                try
                {
                    byte[] buffer = new byte[4096];

                    //Read
                    int bytes = networkStream.Read(buffer, 0, buffer.Length);
                    if (bytes > 0)
                    {
                        data = Encoding.UTF8.GetString(buffer, 0, bytes);
                        Console.WriteLine("---------------------------------");
                        Console.WriteLine(
                            "IP:{0},Port:{1},Message:{2}",
                            endPoint.Address,
                            endPoint.Port,
                            data);

                        //Recording
                        //Jupiter.Utility.SqlDataAccess da = new Jupiter.Utility.SqlDataAccess();
                        //da.GenerateConnection("Data Source=.;Initial Catalog=Jupiter;Integrated Security=True", true);
                        //da.Execute(
                        //    string.Format("INSERT INTO [NetworkRec] VALUES ('{0}','{1}','{2}')",
                        //    endPoint.Address,
                        //    endPoint.Port,
                        //    data));

                        //Write
                        buffer = Encoding.UTF8.GetBytes("SUCESS");
                        networkStream.Write(buffer, 0, buffer.Length);
                    }



                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}\n{1}", ex.Message, ex.StackTrace);
                    break;
                }
            }

        }
    }
}
