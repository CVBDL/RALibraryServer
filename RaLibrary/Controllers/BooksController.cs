using RaLibrary.Data.Entities;
using RaLibrary.Data.Exceptions;
using RaLibrary.Data.Managers;
using RaLibrary.Data.Models;
using RaLibrary.Filters;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace RaLibrary.Controllers
{
    /// <summary>
    /// Books routes.
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/books")]
    [RaAuthentication]
    public class BooksController : RaLibraryController
    {
        #region Fields

        private IBookManager _books = new BookManager();

        #endregion Fields

        /// <summary>
        /// List all books.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public IQueryable<BookDto> ListBooks()
        {
            return _books.List();
        }

        /// <summary>
        /// Get a single book.
        /// </summary>
        /// <param name="id">The book's id.</param>
        /// <returns></returns>
        [Route("{id:int}", Name = "GetSingleBook")]
        [HttpGet]
        [ResponseType(typeof(BookDto))]
        public async Task<IHttpActionResult> GetBook(int id)
        {
            try
            {
                BookDto bookDto = await _books.GetAsync(id);

                return Ok(bookDto);
            }
            catch (DbRecordNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Update a single book.
        /// </summary>
        /// <param name="id">The book's id.</param>
        /// <param name="bookDto">The modified book DTO object.</param>
        /// <returns></returns>
        [Route("{id:int}")]
        [HttpPost]
        [RaLibraryAuthorize(Roles = RoleTypes.Administrators)]
        [ResponseType(typeof(BookDto))]
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
                BookDto result = await _books.UpdateAsync(bookDto);

                return Ok(result);
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
        }

        /// <summary>
        /// Create a book.
        /// </summary>
        /// <param name="bookDto">The new book DTO object.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [RaLibraryAuthorize(Roles = RoleTypes.Administrators)]
        [ResponseType(typeof(Book))]
        public async Task<IHttpActionResult> CreateBook(BookDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                BookDto result = await _books.CreateAsync(bookDto);

                return CreatedAtRoute("GetSingleBook", new { id = result.Id }, result);
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
        }

        /// <summary>
        /// Delete a book.
        /// </summary>
        /// <param name="id">The book's id.</param>
        /// <returns></returns>
        [Route("{id:int}")]
        [HttpDelete]
        [RaLibraryAuthorize(Roles = RoleTypes.Administrators)]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteBook(int id)
        {
            try
            {
                await _books.DeleteAsync(id);

                return StatusCode(HttpStatusCode.NoContent);
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
