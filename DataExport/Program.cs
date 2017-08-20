using Jupiter.Utility;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DataExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure(new FileInfo(Application.StartupPath + "\\log.xml"));

                new Export().Start();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());

                lock (MailUtility.Instance)
                {
                    MailUtility.Instance.SendEmail(Configuration.GetApp("adminEmail"), "数据导出错误", ex.ToString());
                }
            }
        }
    }
}
