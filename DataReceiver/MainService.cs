using Jupiter.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace DataReceiver
{
    public partial class MainService : ServiceBase
    {
        public static TcpClient tcpClient = null;
        public static NetworkStream networkStream = null;
        public static object lockObject = new object();

        Thread netKeeperThread = null,
            transMonThread = null;

        public MainService()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Application.StartupPath + "\\log.xml"));
        }

        protected override void OnStart(string[] args)
        {
            TransmissionMonitor monitor = new TransmissionMonitor(tcpClient, networkStream);
            transMonThread = new Thread(new ThreadStart(monitor.Initilize));
            transMonThread.IsBackground = true;

            NetworkKeeper networkKeeper = new NetworkKeeper(tcpClient, networkStream, transMonThread);
            netKeeperThread = new Thread(new ThreadStart(networkKeeper.Initilize));
            netKeeperThread.IsBackground = true;

            netKeeperThread.Start();
        }

        protected override void OnStop()
        {
            if (transMonThread != null)
            {
                transMonThread.Abort();
            }

            if (netKeeperThread != null)
            {
                netKeeperThread.Abort();
            }

            tcpClient.Close();
            networkStream.Dispose();
        }
    }
}
