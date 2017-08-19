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
                Log.Error(ex.ToString());

                MailUtility.Instance.SendEmail(Configuration.GetApp("adminEmail"), "数据导入错误(计划任务)", ex.ToString());
            }
        }
    }
}
