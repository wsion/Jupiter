using Jupiter.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace Dev
{
    class Program
    {
        private const string SERVER = "S";
        private const string CLIENT = "C";

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Application.StartupPath + "\\log.xml"));

            string cmd = Console.ReadLine();

            switch (cmd.ToUpper())
            {
                case SERVER:
                    NetworkUtility.Server.Start();
                    break;
                case CLIENT:
                    new NetworkUtility.Client().Start();
                    break;
                default:
                    break;
            }
            //Console.ReadLine();
        }
    }
}
