using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Jupiter.Utility;
using Jupiter.DataModel;
using System.IO;
using System.Net;

namespace DataExport
{
    class Export
    {
        MailUtility mail = new MailUtility();

        public void Start()
        {
            var jobs = XmlUtility.DeserializeFromFile<ExportJobs>("job.xml").Items;

            foreach (var job in jobs)
            {
                var DA = new DataAccess(job.DbType, job.ConnectionString);

                var timeStamp = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
                var path = string.Format(@"{0}\output\{1}_{2}.txt", Environment.CurrentDirectory, job.Prefix, timeStamp);
                var pathArch = string.Format(@"{0}\archive\{1}_{2}.txt", Environment.CurrentDirectory, job.Prefix, timeStamp);
                var count = 0;
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    count = DA.LoopSelectResult(
                         job.Query,
                         (col, rowEnd) =>
                         {
                             writer.Write(HttpUtility.UrlEncode(col) + (rowEnd ? "" : "|"));
                         },
                         () =>
                         {
                             writer.WriteLine();
                         });
                }

                upload(path);

                File.Move(path, pathArch);

                mail.SendEmail(Configuration.GetApp("adminEmail"), "数据导出",
                    string.Format("{0}导出{1}条记录", job.SourceName, count));
            }
        }

        private void upload(string path)
        {
            var apiUrl = Configuration.GetApp("apiUrl");
            var client = new WebClient();
            Console.WriteLine("Uploading {0}", path);
            byte[] responseArray = client.UploadFile(apiUrl, path);
            Console.WriteLine("Response: {0}", Encoding.ASCII.GetString(responseArray));
        }
    }
}
