using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jupiter.Utility;

namespace Jupiter.NetworkServices.DataServer
{
    class App
    {
        public static readonly int ServerPort = int.Parse(Configuration.GetApp("ServerPort"));

        public static readonly string DbConnection = Configuration.GetApp("DbConnection");
        public static readonly string DependencyQuery = Configuration.GetApp("DependencyQuery");
        public static readonly string ProcessQuery = Configuration.GetApp("ProcessQuery");
        public static readonly string AfterProcessQuery = Configuration.GetApp("AfterProcessQuery");

        public static readonly string AuthenticationToken = Configuration.GetApp("AuthenticationToken");

    }
}
