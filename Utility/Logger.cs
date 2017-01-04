using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Jupiter.Utility
{
    public class Logger
    {
        public static void Log(string msg)
        {
            var path = string.Format("{0}\\log\\{1}_{2}.log",
                Environment.CurrentDirectory, DateTime.Now.ToString("yyyy_MM_dd"),
                Process.GetCurrentProcess().Id);
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine("[{0}]\r\n{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msg);
                writer.Close();
            }
        }

        public static void Log(string format, params object[] paras)
        {
            Log(string.Format(format, paras));
        }
    }
}
