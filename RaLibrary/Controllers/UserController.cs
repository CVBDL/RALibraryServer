using RaLibrary.Data.Entities;
using RaLibrary.Data.Exceptions;
using RaLibrary.Data.Managers;
using RaLibrary.Data.Models;
using RaLibrary.Filters;
using System.Linq;
using System.Net;
using System.Security.Claims;
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
    public class UserController : ApiController
    {
        #region Fields

        private BookManager books = new BookManager();
        private BorrowLogManager logs = new BorrowLogManager();
        private AdministratorManager administrators = new AdministratorManager();

        #endregion Fields

        /// <summary>
        /// Get user details.
        /// </summary>
        /// <returns></returns>
        [Route("details")]
        [HttpGet]
        public UserDetailsDto GetUserDetails()
        {
            string email = GetClaimEmail();
            string name = GetClaimName();

            return new UserDetailsDto
            {
                Email = email,
                Name = name,
                IsAdmin = administrators.AdministratorExists(email),
            };
        }

        /// <summary>
        /// List the authenticated user borrowed books.
        /// </summary>
        [Route("books")]
        [HttpGet]
        public IQueryable<Book> ListBorrowedBooks()
        {
            string email = GetClaimEmail();

            return books.List().Where(book => book.Borrower == email);
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

            string email = GetClaimEmail();
            try
            {
                await books.UpdateBorrowerAsync(bookDto);
                await logs.CreateAsync(bookDto.Id, email);
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
            string email = GetClaimEmail();

            Book book;
            try
            {
                book = await books.GetAsync(id);
            }
            catch (DbRecordNotFoundException)
            {
                return NotFound();
            }

            if (book.Borrower != email)
            {
                return BadRequest("This books is borrowed by others.");
            }

            BorrowLog log;
            try
            {
                log = logs.GetActive(id);
            }
            catch (DbRecordNotFoundException)
            {
                return NotFound();
            }
            catch (DbOperationException e)
            {
                return BadRequest(e.Message);
            }

            if (log.Borrower != email)
            {
                return BadRequest("This books is borrowed by others.");
            }

            try
            {
                await logs.UpdateAsync(log);
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
                books.Dispose();
                logs.Dispose();
                administrators.Dispose();
            }
            base.Dispose(disposing);
        }

        private string GetClaimEmail()
        {
            ClaimsIdentity identity = User.Identity as ClaimsIdentity;

            return identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
        }

        private string GetClaimName()
        {
            ClaimsIdentity identity = User.Identity as ClaimsIdentity;

            return identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
        }
    }
}
