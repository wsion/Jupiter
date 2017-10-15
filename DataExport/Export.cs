using Jupiter.DataModel;
using Jupiter.Utility;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace DataExport
{
    class Export
    {
        public void Start()
        {
            //1. Read job items from job.xml
            var jobs =
                XmlUtility.DeserializeFromFile<ClientExportJobs>("job.xml").Items;

            if (jobs == null)
            {
                Log.Info("No ClientExportJobs got.");
                return;
            }

            Log.Info("ClientExportJobs got: [{0}] job(s).", jobs.Count);

            foreach (var job in jobs)
            {
                var DA = new DataAccess(job.DbType, job.ConnectionString);

                //2. Generate file
                // - File name: prefix_yyyy_MM-dd_HH_mm_ss.txt
                // - Save to .\output\ folder
                // - Archive to .\archive\ folder
                var filename = string.Format(
                    "{0}_{1}.txt",
                    job.Prefix,
                    DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
                var path = string.Format(
                    @"{0}\output\{1}",
                    Application.StartupPath,
                    filename);
                var pathArch = string.Format(
                    @"{0}\archive\{1}",
                    Application.StartupPath,
                    filename);
                var count = 0;
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    //2.1 Build 'pipe' seperated UrlEncoded data rows
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
                    writer.Close();
                }

                //3. Upload to file server
                var msg = upload(path, filename);

                File.Move(path, pathArch);

                //4. Send notification email
                lock (MailUtility.Instance)
                {
                    MailUtility.Instance.SendEmail(
                        Configuration.GetApp("adminEmail"),
                        "数据导出",
                        string.Format("<b>{0}</b>导出<b>{1}</b>条记录，{2}",
                            job.JobName,
                            count,
                            msg.StatusCode == HttpStatusCode.OK ?
                            "上传文件成功。" : "上传文件失败:" + msg.ToString())
                    );
                }
            }

        }

        private HttpResponseMessage upload(string path, string fileName)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                var client = ApiUtility.GetClient();

                var form = new MultipartFormDataContent();
                var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("form-data")
                    {
                        FileName = fileName
                    };
                form.Add(streamContent);

                Console.WriteLine("Uploading {0}", path);
                var response = client.PostAsync(
                    Configuration.GetApp("apiUrl"),
                    form).Result;
                Log.Info("File [{0}], Response:\r\n{1}", fileName, response);
                stream.Close();
                return response;
            }
        }
    }
}
