using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using Jupiter.Utility;
using DataImport;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize]
    public class StreamController : ApiController
    {
        [HttpPost, Route("api/stream/upload")]
        public HttpResponseMessage Upload()
        {
            //Step 1. Accept txt file, save to specific file folder
            var file = HttpContext.Current.Request.Files[0];
            var ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".txt")
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable);
            }

            var path = Path.Combine(Jupiter.Utility.Configuration.GetApp("FileFolder"), file.FileName);
            file.SaveAs(path);

            //Step 2. Call DataImport module
            importDataAsync(file.FileName);

            return Request.CreateResponse(HttpStatusCode.OK, new { Success = true });
        }

        private void importData(string fileName)
        {
            try
            {
                new Import().Start(fileName);                
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                try
                {
                  MailUtility.Instance.SendEmail(
                        Jupiter.Utility.Configuration.GetApp("adminEmail"), "数据导入错误(Web Host)",
                        ex.ToString());
                }
                catch { }
            }
        }

        private Task importDataAsync(string fileName)
        {
            return Task.Run(() => importData(fileName));
        }
    }
}