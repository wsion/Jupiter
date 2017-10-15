using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jupiter.Utility;
using Jupiter.DataModel;
using System.Text;

namespace WebAPI.Controllers
{
    [Authorize]
    public class DataController : ApiController
    {
        private SqlDataAccess DA = new SqlDataAccess("connection");

        [HttpGet, Route("api/getdata/{jobId}")]
        public HttpResponseMessage GetData(string jobId)
        {
            string result = generateData(jobId);
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(result, System.Text.Encoding.UTF8, "text/plain");
            return resp;
        }

        /// <summary>
        /// Get SqlQuery from ExportSetting by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string getExportQuery(string id)
        {
            var sqls = DA.Query<ServerExportSetting>(
                "SELECT * FROM ExportSetting WHERE Id = @Id",
                new { Id = id });

            if (sqls.Count() > 0)
            {
                return sqls.First().SqlQuery;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Generate plain text using sql statement
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string generateData(string id)
        {
            StringBuilder sb = new StringBuilder();
            var sql = getExportQuery(id);

            DA.ExecuteReader(
                sql, null,
                (record) =>
                {
                    for (int i = 0; i < record.FieldCount; i++)
                    {
                        var col = System.Web.HttpUtility.UrlEncode(record[i].ToString());
                        sb.Append(col + (i == record.FieldCount - 1 ? "" : "|"));
                    }
                    sb.Append(Environment.NewLine);
                });

            return sb.ToString();
        }
    }
}
