using RaLibrary.Data.Entities;
using RaLibrary.Data.Exceptions;
using RaLibrary.Data.Managers;
using RaLibrary.Data.Models;
using RaLibrary.Filters;
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
        #region Fields

        private BookManager _books = new BookManager();

        #endregion Fields

        /// <summary>
        /// List all books.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public IQueryable<BookDto> ListBooks()
        {
            return _books.ListAsDto();
        }

        /// <summary>
        /// Get a book.
        /// </summary>
        /// <param name="id">The book's id.</param>
        /// <returns></returns>
        [Route("{id:int}", Name = "GetSingleBook")]
        [HttpGet]
        [ResponseType(typeof(BookDto))]
        public async Task<IHttpActionResult> GetBook(int id)
        {
            Book book;
            try
            {
                book = await _books.GetAsync(id);
            }
            catch (DbRecordNotFoundException)
            {
                return NotFound();
            }

            return Ok(_books.ToDto(book));
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
        public async Task<IHttpActionResult> UpdateBook(int id, BookDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bookDto.Id)
            {
                return BadRequest("The book id does not match.");
            }

            try
            {
                await _books.UpdateAsync(id, bookDto);
            }
            catch (DbRecordNotFoundException)
            {
                return NotFound();
            }
            catch (DbOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return InternalServerError();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Create a book.
        /// </summary>
        /// <param name="bookDto">The new book.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [RaAuthentication]
        [RaLibraryAuthorize(Roles = RoleTypes.Administrators)]
        [ResponseType(typeof(Book))]
        public async Task<IHttpActionResult> CreateBook(BookDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(bookDto.Borrower))
            {
                bookDto.Borrower = null;
            }

            Book book;
            try
            {
                book = await _books.CreateAsync(bookDto);
            }
            catch (DbRecordNotFoundException)
            {
                return NotFound();
            }
            catch (DbOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return InternalServerError();
            }

            BookDto createdBook = _books.ToDto(book);

            return CreatedAtRoute("GetSingleBook", new { id = createdBook.Id }, createdBook);
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
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteBook(int id)
        {
            try
            {
                await _books.DeleteAsync(id);
            }
            catch (DbRecordNotFoundException)
            {
                return NotFound();
            }
            catch (DbOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return InternalServerError();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _books.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
