using RaLibrary.BooksApi;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace RaLibrary.Controllers
{
    [RoutePrefix("api/book")]
    public class BooksApiController : ApiController
    {
        [Route("isbn/{isbn}")]
        [HttpGet]
        [ResponseType(typeof(BookDetails))]
        public async Task<IHttpActionResult> GetBookByIsbn(string isbn)
        {
            BooksOpenApi doubanBooksApi = new DoubanBooksOpenApi();

            BookDetails bookDetails = null;
            try
            {
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
