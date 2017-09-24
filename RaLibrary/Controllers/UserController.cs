using RaLibrary.Data.Entities;
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
        private IBorrowManager _borrows = new BorrowManager();
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
        [ResponseType(typeof(IQueryable<BorrowDto>))]
        public IHttpActionResult ListBorrowedBooks([FromUri] string borrower = null)
        {
            // require administrator
            if (!string.IsNullOrWhiteSpace(borrower))
            {
                return ListBooksOfUser(borrower);
            }
            else
            {
                string email = ClaimEmail;
                if (string.IsNullOrWhiteSpace(email))
                {
                    var result = new List<BorrowDto>().AsQueryable();
                    return Ok(result);
                }

                return Ok(_borrows.List(email));
            }
        }

        /// <summary>
        /// Borrow a book for the authenticated user.
        /// </summary>
        /// <param name="bookId">The borrowed book id.</param>
        [Route("books/{bookId:int}")]
        [HttpPost]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> BorrowBook(int bookId)
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
                Borrow borrow = new Borrow()
                {
                    FK_Book_Id = bookId,
                    Borrower = email
                };
                await _borrows.CreateAsync(borrow);

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
        [Route("books/{bookId:int}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> ReturnBook(int bookId)
        {
            string email = ClaimEmail;
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Cannot identify you.");
            }

            Borrow borrow;
            try
            {
                borrow = _borrows.GetByBookId(bookId);
            }
            catch (DbRecordNotFoundException)
            {
                return NotFound();
            }
            catch (DbOperationException e)
            {
                return BadRequest(e.Message);
            }

            if (borrow.Borrower != email)
            {
                return BadRequest("This books is borrowed by others.");
            }

            try
            {
                await _borrows.DeleteAsync(borrow.Id);
            }
            catch (DbOperationException e)
            {
                return BadRequest(e.Message);
            }

            try
            {
                BorrowLogDto borrowLogDto = new BorrowLogDto()
                {
                    F_BookID = bookId,
                    Borrower = email,
                    BorrowTime = borrow.BorrowTime,
                    ExpectedReturnTime = borrow.ExpectedReturnTime
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _books.Dispose();
                _borrows.Dispose();
                _logs.Dispose();
                _administrators.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult ListBooksOfUser(string borrower)
        {
            if (IsAdministrator)
            {
                return Ok(_borrows.List(borrower));
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
