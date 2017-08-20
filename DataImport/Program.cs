using Jupiter.Utility;
using System;
using System.IO;

namespace DataImport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure(new FileInfo(Application.StartupPath + "\\log.xml"));
                Log.Info(args[0]);

                var fileName = args[0];
                new Import().Start(fileName);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());

                lock (MailUtility.Instance)
                {
                    MailUtility.Instance.SendEmail(Configuration.GetApp("adminEmail"), "数据导入错误(计划任务)", ex.ToString());
                }
            }
        }
    }
}
