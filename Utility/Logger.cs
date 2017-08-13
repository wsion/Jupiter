using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Jupiter.Utility
{
    public class Logger
    {
        public static void Log(string msg)
        {
            var rootFolder = Configuration.GetApp("log");
            if (rootFolder == null)
            {
                rootFolder = Application.StartupPath + "\\log";
            }
            var path = string.Format("{0}\\{1}_{2}.log",
                rootFolder, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"),
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
