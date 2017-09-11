using RaLibrary.BooksApi;
using RaLibrary.Utils;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace RaLibrary.Controllers
{
    [RoutePrefix("api/book")]
    public class BooksApiController : ApiController
    {
        [Route("isbn/{strIsbn}")]
        [HttpGet]
        [ResponseType(typeof(BookDetails))]
        public async Task<IHttpActionResult> GetBookByIsbn(string strIsbn)
        {
            Isbn isbn;
            try
            {
                isbn = new Isbn(strIsbn);
            }
            catch (IsbnFormatException)
            {
                return BadRequest("Invalid ISBN format.");
            }

            BookDetails bookDetails;
            try
            {
                IBooksOpenApi doubanBooksApi = new DoubanBooksOpenApi();
                bookDetails = await doubanBooksApi.QueryIsbnAsync(isbn);
            }
            catch (BookNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            if (bookDetails != null)
            {
                return Ok(bookDetails);
            }
            else
            {
                return InternalServerError();
            }
        }
    }
}
