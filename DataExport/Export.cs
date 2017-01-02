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

        public void Start()
        {
            var jobs = this.getSettings();

            foreach (var job in jobs)
            {
                var DA = new DataAccess(job.DbType, job.ConnectionString);

                var path = string.Format(@"{0}\output\{1}_{2}.txt", Environment.CurrentDirectory, job.Prefix, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    DA.LoopSelectResult(
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
            }

            //todo: Archive file
            //todo: Email alert
        }

        private List<ExportJob> getSettings()
        {
            using (StreamReader reader = new StreamReader(Environment.CurrentDirectory + "\\job.xml"))
            {
                var xml = reader.ReadToEnd();
                var list = XmlUtility.DeserializeObject<ExportJobs>(xml);
                return list.Items;
            }
        }

        private void upload(string path)
        {
            var apiUrl = Configuration.GetApp("apiUrl");
            var client = new WebClient();
            Console.WriteLine("Uploading {0}", path);
            byte[] responseArray = client.UploadFile(apiUrl, path);
            Console.WriteLine("Response: {0}", Encoding.Unicode.GetString(responseArray));
        }
    }
}
