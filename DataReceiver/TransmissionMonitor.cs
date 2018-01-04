using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Jupiter.Utility;
using System.Threading;

namespace DataReceiver
{
    class TransmissionMonitor
    {
        //TcpClient tcpClient;
        //NetworkStream networkStream;
        private object lockObject = new object();

        public TransmissionMonitor(TcpClient tcpClient, NetworkStream networkStream)
        {
            //this.tcpClient = tcpClient;
            //this.networkStream = networkStream;
        }

        public void Initilize()
        {
            while (true)
            {
                //lock (MainService.lockObject)
                //{
                if (MainService.tcpClient == null || MainService.networkStream == null ||
                    !MainService.tcpClient.Connected || !MainService.networkStream.CanRead)
                {
                    break;
                }

                string text = string.Empty;

                try
                {
                    byte[] buffer = new byte[Constants.TCP_BUFFER_SIZE];
                    int bytes = MainService.networkStream.Read(buffer, 0, buffer.Length);
                    if (bytes > 0)
                    {
                        text = Encoding.UTF8.GetString(buffer, 0, bytes);
                    }
                }
                catch (Exception ex)
                {
                    Thread.CurrentThread.Suspend();
                    Log.Error("Receiving text failed.\r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
                }

                Log.Info("Received text data:{0}", text);
                //}
            }
        }
    }
}
