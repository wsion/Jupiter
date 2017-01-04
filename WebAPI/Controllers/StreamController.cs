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

namespace WebAPI.Controllers
{
    [Authorize]
    public class StreamController : ApiController
    {
        [HttpPost, Route("api/stream/upload")]
        public HttpResponseMessage Upload()
        {
            var file = HttpContext.Current.Request.Files[0];
            var ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".txt")
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable);
            }

            var path = Path.Combine(Jupiter.Utility.Configuration.GetApp("FileFolder"), file.FileName);
            file.SaveAs(path);

            return Request.CreateResponse(HttpStatusCode.OK, new { Success = true });
        }
    }
}