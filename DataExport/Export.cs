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
using System.Net.Http;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace DataExport
{
    class Export
    {
        string token;
        MailUtility mail = new MailUtility();
        string apiHost;

        public Export()
        {
            apiHost = Configuration.GetApp("apiHost");
        }

        public void Start()
        {
            var jobs = XmlUtility.DeserializeFromFile<ExportJobs>("job.xml").Items;

            foreach (var job in jobs)
            {
                var DA = new DataAccess(job.DbType, job.ConnectionString);

                var filename = string.Format("{0}_{1}.txt", job.Prefix, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
                var path = string.Format(@"{0}\output\{1}", Environment.CurrentDirectory, filename);
                var pathArch = string.Format(@"{0}\archive\{1}", Environment.CurrentDirectory, filename);
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
                    writer.Close();
                }

                var msg = upload(path, filename);

                File.Move(path, pathArch);

                mail.SendEmail(Configuration.GetApp("adminEmail"), "数据导出",
                    string.Format("<b>{0}</b>导出<b>{1}</b>条记录，{2}", job.SourceName, count,
                    msg.StatusCode == HttpStatusCode.OK ? "上传文件成功。" : "上传文件失败:" + msg.ToString()));
            }

        }

        private HttpResponseMessage upload(string path, string fileName)
        {
            getToken();

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(apiHost);

                var form = new MultipartFormDataContent();
                var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = fileName
                };
                form.Add(streamContent);

                Console.WriteLine("Uploading {0}", path);
                //byte[] responseArray = client.UploadFile(apiUrl, path);
                var response = client.PostAsync(Configuration.GetApp("apiUrl"), form).Result;
                Logger.Log("File [{0}], Response:\r\n{1}", fileName, response);
                stream.Close();
                return response;
            }
        }

        private void getToken()
        {
            if (!string.IsNullOrEmpty(token))
            {
                return;
            }

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(apiHost);

            var parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "client_credentials");

            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(Constants.ClientID + ":" + Constants.ClientSecrect)
                        ));

            var response = httpClient.PostAsync("token", new FormUrlEncodedContent(parameters)).Result;
            var responseValue = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                token = JObject.Parse(responseValue)["access_token"].Value<string>();
            }
        }
    }
}
