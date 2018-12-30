using MicroService4Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using static System.Net.Mime.MediaTypeNames;

namespace GaizerMicoService
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

