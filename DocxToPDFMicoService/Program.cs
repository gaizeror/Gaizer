using MicroService4Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace DocxToPDFMicoService
{
    public class Program
    {
        static void Main(string[] args)
        {
            var microService = new MicroService();
            microService.Run(args);
        }
    }
}

