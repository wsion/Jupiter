using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Jupiter.Utility
{
    public class Log
    {
        private static readonly ILog log = LogManager.GetLogger("TextLogger");

        public static void Warn(string message)
        {
            log.Warn(message);
        }

        public static void Info(string message)
        {
            log.Info(message);
        }

        public static void Info(string format, params object[] para)
        {
            log.Info(string.Format(format, para));
        }
        public static void Fatal(string message)
        {
            log.Fatal(message);
        }

        public static void Error(string message)
        {
            log.Error(message);
        }

        public static void Debug(string message)
        {
            log.Debug(message);
        }
    }
}
