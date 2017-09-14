using RaLibrary.Data.Context;
using RaLibrary.Data.Entities;
using RaLibrary.Data.Managers;
using RaLibrary.Data.Models;
using RaLibrary.Filters;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace RaLibrary.Controllers
{
    /// <summary>
    /// Books routes.
    /// </summary>
    [RoutePrefix("api/books")]
    public class BooksController : ApiController
    {
        private RaLibraryContext db = new RaLibraryContext();
        private BookManager books = new BookManager();

        /// <summary>
        /// List all books.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public IQueryable<Book> ListBooks()
        {
            return books.List();
        }

        /// <summary>
        /// Get a book.
        /// </summary>
        /// <param name="id">The book's id.</param>
        /// <returns></returns>
        [Route("{id:int}", Name = "GetSingleBook")]
        [HttpGet]
        [ResponseType(typeof(Book))]
        public async Task<IHttpActionResult> GetBook(int id)
        {
            Book book = await books.Get(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        /// <summary>
        /// Update a single book.
        /// </summary>
        /// <param name="id">The book's id.</param>
        /// <param name="bookDto">The updated book.</param>
        /// <returns></returns>
        [Route("{id:int}")]
        [HttpPost]
        [RaAuthentication]
        [RaLibraryAuthorize(Roles = RoleTypes.Administrators)]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> UpdateBook(int id, BookDTO bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bookDto.Id)
            {
                return BadRequest();
            }

            try
            {
                await books.Update(id, bookDto);
            }
            catch (BookRecordNotFoundException)
            {
                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Create a book.
        /// </summary>
        /// <param name="book">The new book.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [RaAuthentication]
        [RaLibraryAuthorize(Roles = RoleTypes.Administrators)]
        [ResponseType(typeof(Book))]
        public async Task<IHttpActionResult> CreateBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Borrower))
            {
                book.Borrower = null;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Books.Add(book);

            await db.SaveChangesAsync();

            return CreatedAtRoute("GetSingleBook", new { id = book.Id }, book);
        }

        /// <summary>
        /// Delete a book.
        /// </summary>
        /// <param name="id">The book's id.</param>
        /// <returns></returns>
        [Route("{id:int}")]
        [HttpDelete]
        [RaAuthentication]
        [RaLibraryAuthorize(Roles = RoleTypes.Administrators)]
        [ResponseType(typeof(Book))]
        public async Task<IHttpActionResult> DeleteBook(int id)
        {
            Book book = await db.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            db.Books.Remove(book);

            await db.SaveChangesAsync();

            return Ok(book);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
