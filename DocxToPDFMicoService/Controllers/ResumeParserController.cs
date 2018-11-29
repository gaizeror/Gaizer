using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DocxToPDFMicoService.Controllers
{
    public class ResumeParserController : ApiController
    {
        [Route("api/ping")]
        public string GetPing()
        {
            return DateTime.Now.ToString();
        }

        //[HttpPost]
        //[Route("convert/pdf")]
        //public string ss()
        //{
        //    return null;
        //}
    }
}
