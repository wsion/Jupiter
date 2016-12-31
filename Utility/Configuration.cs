using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Utility
{
    public class Configuration
    {
        public static string GetConnection(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ToString();
        }

        public static string GetApp(string name)
        {
            return ConfigurationManager.AppSettings[name].ToString();
        }
    }
}
