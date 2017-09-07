using RaLibrary.BooksApi;
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
            BookDetails bookDetails = await doubanBooksApi.QueryIsbnAsync(isbn);

            return Ok(bookDetails);
        }
    }
}
