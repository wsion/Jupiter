using Jupiter.Utility;
using System;

namespace DataExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new Export().Start();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());

                try
                {
                    new MailUtility().SendEmail(Configuration.GetApp("adminEmail"), "数据导出错误", ex.ToString());
                }
                catch { }
            }
        }
    }
}
