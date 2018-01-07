using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jupiter.Utility;

namespace Jupiter.NetworkServices.DataReceiver
{
    class App
    {
        public static readonly string ServerIp = Configuration.GetApp("ServerIp");
        public static readonly int ServerPort = int.Parse(Configuration.GetApp("ServerPort"));

        public static readonly string DbConnection = Configuration.GetApp("DbConnection");
        public static readonly string DbType = Configuration.GetApp("DbType");
        public static readonly string DbParameterPrifix = Configuration.GetApp("DbParameterPrifix");
        public static readonly string ProcessDataQuery = Configuration.GetApp("ProcessDataQuery");


        public static readonly string AuthenticationToken = Configuration.GetApp("AuthenticationToken");
        public static readonly string AuthenticationIdentity = Configuration.GetApp("AuthenticationIdentity");
    }
}
