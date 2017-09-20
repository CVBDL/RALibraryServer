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
    /// User routes.
    /// <https://github.com/CVBDL/RALibraryDocs/blob/master/rest-api.md#users>
    /// </summary>
    [RoutePrefix("api/user")]
    [RaAuthentication]
    [RaLibraryAuthorize(Roles = RoleTypes.NormalUsers)]
    public class UserController : RaLibraryController
    {
        #region Fields

        private BookManager _books = new BookManager();
        private BorrowLogManager _logs = new BorrowLogManager();
        private AdministratorManager _administrators = new AdministratorManager();

        #endregion Fields

        /// <summary>
        /// Get user details.
        /// </summary>
        /// <returns></returns>
        [Route("details")]
        [HttpGet]
        public UserDetailsDto GetUserDetails()
        {
            string email = ClaimEmail;
            string name = ClaimName;

            return new UserDetailsDto
            {
                Email = email,
                Name = name,
                IsAdmin = _administrators.AdministratorExists(email),
            };
        }

        /// <summary>
        /// List the authenticated user borrowed books.
        /// </summary>
        [Route("books")]
        [HttpGet]
        public IQueryable<BookDto> ListBorrowedBooks()
        {
            string email = ClaimEmail;

            return _books.List().Where(book => book.Borrower == email);
        }

        /// <summary>
        /// Borrow a book for the authenticated user.
        /// </summary>
        /// <param name="book">The borrowed book.</param>
        [Route("books")]
        [HttpPost]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> BorrowBook(BookDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string email = ClaimEmail;
            try
            {
                await _books.UpdateBorrowerAsync(bookDto);

                BorrowLogDto borrowLogDto = new BorrowLogDto()
                {
                    F_BookID = bookDto.Id,
                    Borrower = email
                };
                await _logs.CreateAsync(borrowLogDto);
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
        /// Return a book for the authenticated user.
        /// </summary>
        /// <param name="id">The book's id.</param>
        [Route("books/{id:int}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> ReturnBook(int id)
        {
            string email = ClaimEmail;

            BookDto bookDto;
            try
            {
                bookDto = await _books.GetAsync(id);
            }
            catch (DbRecordNotFoundException)
            {
                return NotFound();
            }

            if (bookDto.Borrower != email)
            {
                return BadRequest("This books is borrowed by others.");
            }

            BorrowLogDto logDto;
            try
            {
                logDto = _logs.GetActive(id);
            }
            catch (DbRecordNotFoundException)
            {
                return NotFound();
            }
            catch (DbOperationException e)
            {
                return BadRequest(e.Message);
            }

            if (logDto.Borrower != email)
            {
                return BadRequest("This books is borrowed by others.");
            }

            try
            {
                await _logs.UpdateAsync(logDto);
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
                _logs.Dispose();
                _administrators.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
