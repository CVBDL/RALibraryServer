using RaLibrary.Data.Managers;
using RaLibrary.Data.Models;
using RaLibrary.Filters;
using RaLibrary.Utilities.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RaLibrary.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/report")]
    [RaAuthentication]
    public class ReportController : ApiController
    {
        IReportManager _reportManager = new ReportManager();

        /// <summary>
        /// List all books.
        /// </summary>
        /// <returns></returns>
        [Route("books/{type}")]
        [HttpGet]
        public HttpResponseMessage GetBooksReport(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            var dtos = _reportManager.GetAllBooksStatusReport() as List<BookStateDto>;
            var fileName = "BookStatusReport_" + DateTime.Now.ToString("yyyyMMdd");
            byte[] buffer = null;
            switch (type.ToLower())
            {
                case "csv":
                    fileName += ".csv";
                    buffer = GetCsvFileContent(fileName, dtos);
                    break;
                case "excel":
                    fileName += ".xlsx";
                    buffer = GetExcelFileContent(fileName, dtos);
                    break;
                default:
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            var fileContent = new ByteArrayContent(buffer);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = fileContent
            };
        }

        private byte[] GetCsvFileContent(string fileName, List<BookStateDto> dtos)
        {
            if (dtos == null)
                return null;

            var builder = new StringBuilder();
            // add head
            builder.AppendLine("Bar Code,Book Name,Book Status,Borrower,Borrowed Date,Expected Returning Date");
            // add content
            foreach (var dto in dtos)
            {
                builder.AppendLine($"{dto.Code},{dto.Name},{dto.Status},{dto.Borrower},{dto.BorrowedDate},{dto.ExpectedReturnDate}");
            }
            var content = builder.ToString();
            var buffer =  Encoding.UTF8.GetBytes(content);
            return Encoding.UTF8.GetPreamble().Concat(buffer).ToArray();
        }

        private byte[] GetExcelFileContent(string fileName, List<BookStateDto> dtos)
        {
            if (dtos == null)
                return null;

            var header = new string[] { "Bar Code", "Book Name", "Book Status", "Borrower", "Borrowed Date", "Expected Returning Date" };
            var excel = new ExcelFile();
            excel.Create();
            var sheet = excel.AddSheet();
            for (var i = 0; i < header.Length; i++)
            {
                sheet.WriteCell(0, i, header[i]);
            }
            for (var i = 0; i < dtos.Count; i++)
            {
                var row = i + 1;
                var col = 0;
                sheet.WriteCell(row, col++, dtos[i].Code);
                sheet.WriteCell(row, col++, dtos[i].Name);
                sheet.WriteCell(row, col++, dtos[i].Status.ToString());
                sheet.WriteCell(row, col++, dtos[i].Borrower);
                sheet.WriteCell(row, col++, dtos[i].BorrowedDate);
                sheet.WriteCell(row, col++, dtos[i].ExpectedReturnDate);
            }

            var path = string.Empty;
            try
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + fileName;
            }
            catch (Exception)
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + fileName;
            }
            excel.SaveAs(path);
            excel.Close();
            var buffer = File.ReadAllBytes(path);
            File.Delete(path);
            return buffer;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _reportManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
