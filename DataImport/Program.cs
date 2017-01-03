using Jupiter.Utility;
using System;

namespace DataImport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new Import().Start();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());

                try
                {
                    new MailUtility().SendEmail(Configuration.GetApp("adminEmail"), "数据导入错误", ex.ToString());
                }
                catch { }
            }
        }
    }
}
