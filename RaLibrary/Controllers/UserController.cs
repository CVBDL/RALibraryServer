using RaLibrary.Data.Exceptions;
using RaLibrary.Data.Managers;
using RaLibrary.Data.Models;
using RaLibrary.Filters;
using System.Collections.Generic;
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

        private IBookManager _books = new BookManager();
        private IBorrowLogManager _logs = new BorrowLogManager();
        private IAdministratorManager _administrators = new AdministratorManager();

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
                IsAdmin = _administrators.IsAdministrator(email),
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
            if (string.IsNullOrWhiteSpace(email))
            {
                return new List<BookDto>().AsQueryable();
            }

            return _books.List(email);
        }

        /// <summary>
        /// Borrow a book for the authenticated user.
        /// </summary>
        /// <param name="id">The borrowed book id.</param>
        [Route("books/{id:int}")]
        [HttpPost]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> BorrowBook(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string email = ClaimEmail;
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Cannot identify you.");
            }

            try
            {
                BorrowLogDto borrowLogDto = new BorrowLogDto()
                {
                    F_BookID = id,
                    Borrower = email
                };
                await _logs.CreateAsync(borrowLogDto);

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
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Cannot identify you.");
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
                await _logs.CloseAsync(logDto);

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
                _logs.Dispose();
                _administrators.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
