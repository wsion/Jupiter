using Jupiter.Utility;
using System;
using System.Threading.Tasks;

namespace DataExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Task<string> task = Task.Run(() => new Export().Start());
                //var result = task.Result;
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
