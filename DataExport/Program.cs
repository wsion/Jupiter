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
            }
            catch (Exception ex)
            {
                emailError(ex);
            }

            try
            {
                new Export().Start();
            }
            catch (Exception ex)
            {
                emailError(ex);
            }

            try
            {
                new Import().Start();
            }
            catch (Exception ex)
            {
                emailError(ex);
            }
        }

        private static void emailError(Exception ex)
        {
            Log.Error(ex.ToString());

            lock (MailUtility.Instance)
            {
                MailUtility.Instance.SendEmail(Configuration.GetApp("adminEmail"), "数据导出/导入错误", ex.ToString());
            }
        }
    }
}
