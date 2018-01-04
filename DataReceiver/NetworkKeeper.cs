using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Jupiter.Utility;

namespace DataReceiver
{
    class NetworkKeeper : IDisposable
    {
        //TcpClient tcpClient;
        //NetworkStream networkStream;
        Timer timer;
        Thread transMonThread = null;

        public NetworkKeeper(TcpClient tcpClient, NetworkStream networkStream, Thread transMonThread)
        {
            //this.tcpClient = tcpClient;
            //this.networkStream = networkStream;
            this.transMonThread = transMonThread;
            transMonThread.Start();
        }

        public void Initilize()
        {
            initializeTcpClient();
            timer = new Timer(Timer_Elapsed, null, 0, 1000);
        }

        private void Timer_Elapsed(object obj)
        {
            lock (MainService.lockObject)
            {
                if (!NetworkStreamUtility.SendText("0", MainService.networkStream, 1))
                {
                    transMonThread.Suspend();
                    initializeTcpClient();
                }
            }
        }

        private void initializeTcpClient()
        {
            try
            {
                MainService.tcpClient = new TcpClient();
                MainService.tcpClient.Connect(IPAddress.Parse("118.190.117.0"), 8888);
                MainService.networkStream = MainService.tcpClient.GetStream();
                if(transMonThread.ThreadState == ThreadState.Suspended)
                {
                    transMonThread.Start();
                }
            }
            catch (Exception ex)
            {
                Log.Error("{0}\r\n{1}", ex.Message, ex.StackTrace);
                transMonThread.Suspend();
            }
        }

        public void Dispose()
        {
            this.timer.Dispose();
        }
    }
}
