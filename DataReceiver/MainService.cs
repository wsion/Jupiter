using Jupiter.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace DataReceiver
{
    public partial class MainService : ServiceBase
    {
        DataClient dataClient;

        public MainService()
        {
            InitializeComponent();

            log4net.Config.XmlConfigurator.Configure(new FileInfo(Application.StartupPath + "\\log.xml"));
        }

        protected override void OnStart(string[] args)
        {
            dataClient = new DataClient();
            dataClient.Start();
        }

        protected override void OnStop()
        {
            dataClient.Dispose();
        }
    }
}
