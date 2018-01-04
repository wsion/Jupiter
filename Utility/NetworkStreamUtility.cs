using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Jupiter.Utility
{
    public class NetworkStreamUtility
    {
        public static bool SendText(string text, NetworkStream stream, int bufferSizeMax = Constants.TCP_BUFFER_SIZE)
        {
            bool result = false;

            if (text.Length > bufferSizeMax)
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

        public static string ReceiveText(NetworkStream stream, int bufferSizeMax = Constants.TCP_BUFFER_SIZE)
        {
            string text = string.Empty;

            try
            {
                byte[] buffer = new byte[bufferSizeMax];
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
    }
}
