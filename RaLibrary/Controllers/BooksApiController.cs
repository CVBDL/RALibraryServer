using RaLibrary.BookApiProxy;
using RaLibrary.BookApiProxy.Douban;
using RaLibrary.BookApiProxy.Exceptions;
using RaLibrary.BookApiProxy.Models;
using RaLibrary.Utilities;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace RaLibrary.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/book")]
    public class BooksApiController : ApiController
    {
        [Route("isbn/{strIsbn}")]
        [HttpGet]
        [ResponseType(typeof(BookDetailsDto))]
        public async Task<IHttpActionResult> GetBookByIsbn(string strIsbn)
        {
            Isbn isbn;
            try
            {
                isbn = new Isbn(strIsbn);
            }
            catch (IsbnFormatException e)
            {
                return BadRequest(e.Message);
            }

            BookDetailsDto bookDetails;
            try
            {
                IBooksOpenApi doubanBooksApi = new DoubanBooksOpenApi();
                bookDetails = await doubanBooksApi.QueryIsbnAsync(isbn);
            }
            catch (BookNotFoundException)
            {
                return NotFound();
            }
            catch
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
