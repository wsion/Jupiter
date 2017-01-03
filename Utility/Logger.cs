using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Jupiter.Utility
{
    public class Logger
    {
        public static void Log(string msg)
        {
            var path = string.Format("{0}\\log\\{1}.log", Environment.CurrentDirectory, DateTime.Now.ToString("yyyy_MM_dd"));
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine("[{0}]\r\n{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msg);
            }
        }
    }
}
