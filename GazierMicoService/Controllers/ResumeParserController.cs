using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GaizerMicoService.Controllers
{
    public class ResumeParserController : ApiController
    {
        private static readonly string ServerUploadFolder = Path.GetTempPath();

        [Route("api/ping")]
        public string GetPing()
        {
            return DateTime.Now.ToString();
        }

        [HttpPost]
        [Route("api/convert")]
        public async Task<string> ConvertAsync(string file_name)
        {
            //var streamProvider = new MultipartFormDataStreamProvider(ServerUploadFolder);
            //MultipartFormDataStreamProvider s = await Request.Content.ReadAsMultipartAsync(streamProvider);
            //StreamContent ss = this.StreamConversion();

            string docxFilePath = GetDocxFilePath(file_name);

            await WriteFileToLocalDocx(docxFilePath);

            

            //StreamContent ss = this.StreamConversion(s);
            // GazierConverter.Converter.Convert()

            return null;
        }

        private async Task WriteFileToLocalDocx(string docxFilePath)
        {
            Stream reqStream = await Request.Content.ReadAsStreamAsync();
            WriteToFile(reqStream, docxFilePath);
        }

        private static string GetDocxFilePath(string fileName)
        {
            string docxUniqueFileName = fileName + Guid.NewGuid();
            string docxFilePath = Path.Combine(Path.GetTempPath(), docxUniqueFileName);
            return docxFilePath;
        }

        private static void WriteToFile(Stream reqStream,string path)
        {
            using (MemoryStream tempStream = new MemoryStream())
            {
                reqStream.CopyTo(tempStream);
                using (var fileStream = File.Create(path))
                {
                    tempStream.Seek(0, SeekOrigin.Begin);
                    tempStream.CopyTo(fileStream);
                }
            }
        }

        private StreamContent StreamConversion()
        {
            Stream reqStream = Request.Content.ReadAsStreamAsync().Result;
            var tempStream = new MemoryStream();
            reqStream.CopyTo(tempStream);

            tempStream.Seek(0, SeekOrigin.End);
            var writer = new StreamWriter(tempStream);
            writer.WriteLine();
            writer.Flush();
            tempStream.Position = 0;

            var streamContent = new StreamContent(tempStream);
            foreach (var header in Request.Content.Headers)
            {
                streamContent.Headers.Add(header.Key, header.Value);
            }
            return streamContent;
        }
    }
}
