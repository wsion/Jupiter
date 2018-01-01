using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Jupiter.Utility;

namespace DataReceiver
{
    class DataClient : IDisposable
    {
        TcpClient tcpClient;
        NetworkStream networkStream;
        Timer timer;
        const int MAX_BUFFER_SIZE = 65536;

        public void Start()
        {
            reConnect();
            initializeTimer();
        }

        private void initializeTimer()
        {
            timer = new Timer(Timer_Elapsed, null, 0, 1000);
        }

        private void Timer_Elapsed(object obj)
        {
            sendData();
        }

        private void reConnect()
        {
            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect(IPAddress.Parse("118.190.117.0"), 8888);
                networkStream = tcpClient.GetStream();
            }
            catch (Exception ex)
            {
                Log.Error("{0}\r\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private void sendData()
        {
            testConnection();
            sendText(System.DateTime.Now.ToString(), this.networkStream);
        }

        private void testConnection()
        {
            if (!sendText("0", this.networkStream))
            {
                reConnect();
            }
        }

        private bool sendText(string text, NetworkStream stream)
        {
            bool result = false;

            if (text.Length > MAX_BUFFER_SIZE)
            {
                return result;
            }

            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                stream.Write(buffer, 0, buffer.Length);
                result = true;
            }
            catch (Exception ex)
            {
                Log.Error("Sending text failed.\r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
                result = false;
            }

            return result;
        }

        private string receiveText(NetworkStream stream)
        {
            string text = string.Empty;

            try
            {
                byte[] buffer = new byte[MAX_BUFFER_SIZE];
                int bytes = stream.Read(buffer, 0, buffer.Length);
                if (bytes > 0)
                {
                    text = Encoding.UTF8.GetString(buffer, 0, bytes);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Receiving text failed.\r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
            }

            return text;
        }

        public void Dispose()
        {
            this.timer.Dispose();
            this.tcpClient.Close();
        }
    }
}
