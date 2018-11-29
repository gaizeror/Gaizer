using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public HttpResponseMessage ConvertAsync(string file_name)
        {
            string docxFilePath = GetDocxFilePath(file_name);

            string pdfOutputPath = SaveAsPDF(docxFilePath);

            return ComposePDFHttpResult(file_name, pdfOutputPath);
        }

        private static string SaveAsPDF(string docxFilePath)
        {
            string inputFilePath = docxFilePath;
            var filename = Path.GetFileNameWithoutExtension(inputFilePath);
            var parentDir = Path.GetDirectoryName(inputFilePath);
            var outputPath = Path.Combine(parentDir, filename + ".pdf");

            GazierConverter.Converter.ConvertToPdf(inputFilePath, filename, outputPath);

            return outputPath;
        }

        private static string GetDocxFilePath(string fileName)
        {
            string docxUniqueFileName = fileName + Guid.NewGuid();
            string docxFilePath = Path.Combine(Path.GetTempPath(), docxUniqueFileName);
            return docxFilePath;
        }

        private static HttpResponseMessage ComposePDFHttpResult(string file_name, string pdfOutputPath)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(pdfOutputPath, FileMode.Open); // Read the PDF from file
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = file_name;
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentLength = stream.Length;
            return result;
        }




        private async Task WriteFileToLocalDocx(string docxFilePath)
        {
            Stream reqStream = await Request.Content.ReadAsStreamAsync();
            WriteToFile(reqStream, docxFilePath);
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
