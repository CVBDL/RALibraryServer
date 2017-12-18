using RaLibrary.Data.Managers;
using RaLibrary.Data.Models;
using RaLibrary.Filters;
using RaLibrary.Utilities.Excel;
using System;
using System.Collections.Generic;
using System.IO;
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
            var dtos = _reportManager.GetAllBooksStatusReport() as List<BookStateDto>;
            var fileName = "BookStatusReport_" + DateTime.Now.ToString("yyyyMMdd");
            byte[] buffer = null;
            switch (type)
            {
                case "csv":
                    fileName += ".csv";
                    buffer = GetCsvFileContent(fileName, dtos);
                    break;
                case "excel":
                    fileName += ".xlsx";
                    buffer = GetExcelFileContent(fileName, dtos);
                    break;
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
            // can't use UTF-8 or ASCII charset for the content, otherwise the EXCEL will not correctly parse the content and chinese
            // character will display wrong format.
            return Encoding.Default.GetBytes(content);
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
            for (var i =  0; i < dtos.Count; i++)
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
            // TODO: find a temp file folder.
            var path = @"d:\" + fileName;
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
