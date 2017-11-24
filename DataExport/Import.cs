using Jupiter.DataModel;
using Jupiter.Utility;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace DataExport
{
    class Import
    {
        public void Start()
        {
            //1. Read job items from job.xml
            var jobs =
                XmlUtility.DeserializeFromFile<ClientImportJobs>("job.xml").Items;

            if (jobs == null)
            {
                Log.Info("No ClientImportJobs got.");
                return;
            }

            Log.Info("ClientImportJobs got: [{0}] job(s).", jobs.Count);

            //2. Process each job
            foreach (var job in jobs)
            {
                //2.1 Request data from server
                var textData = getPlainText(job.ApiUrl);

                //2.2 Process data
                if (textData != string.Empty)
                {
                    processData(textData, job);
                }
            }
        }

        private string getPlainText(string apiUrl)
        {
            using (var client = ApiUtility.GetClient())
            {
                var response = client.GetAsync(apiUrl).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StreamContent content = (StreamContent)response.Content;
                    var result = content.ReadAsStringAsync().Result;
                    Log.Info("Data got from WebApi: [{0}].", apiUrl);
                    return result;
                }
            }

            Log.Info("No data got from WebApi: [{0}].", apiUrl);
            return string.Empty;
        }

        private void processData(string text, ClientImportJob job)
        {
            var DA = new DataAccess(job.DbType, job.ConnectionString);
            var count = 0;

            Log.Info("Job [{0}] Started.", job.JobName);

            //a. Get rows
            string[] rows = text.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None);

            //b. process single row
            foreach (var line in rows)
            {
                if (line.Trim() == string.Empty)
                {
                    continue;
                }

                List<KeyValuePair<string, string>> parameters =
                    new List<KeyValuePair<string, string>>();

                //c. Get columns
                string[] cols = line.Split(
                                new[] { "|" },
                                StringSplitOptions.None);

                for (int i = 0; i < cols.Length; i++)
                {
                    //d. Build parameters named @p0,@p1,@p2 etc.
                    //todo: support DB other than MSSQL
                    var key = job.ParameterPrefix + i;
                    var value =
                    HttpUtility.UrlDecode(cols[i]);
                    KeyValuePair<string, string> parameter =
                        new KeyValuePair<string, string>(key, value);
                    parameters.Add(parameter);
                }

                //e. Excute SQL defined in job
                DA.ExecuteNonQuery(job.Query, parameters);
                count++;

                var decodedLine = HttpUtility.UrlDecode(line);
                Console.WriteLine(decodedLine);                
            }

            Log.Info("Data execution succedded: [{0}] rows.", count);

            //f. Send notification email
            lock (MailUtility.Instance)
            {
                MailUtility.Instance.SendEmail(
                    Configuration.GetApp("adminEmail"),
                    "数据导出",
                    string.Format("<b>{0}</b>导入<b>{1}</b>条记录.",
                        job.JobName,
                        count)
                );
            }
        }
    }
}
