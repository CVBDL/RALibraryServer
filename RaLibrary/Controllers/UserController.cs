using RaLibrary.Filters;
using RaLibrary.Models;
using RaLibrary.Utils;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
    public class UserController : ApiController
    {
        private RaLibraryContext db = new RaLibraryContext();

        /// <summary>
        /// Get user details.
        /// </summary>
        /// <returns></returns>
        [Route("details")]
        [HttpGet]
        [RaAuthentication]
        [RaLibraryAuthorize(Roles = RoleTypes.NormalUsers)]
        public UserDetailsDTO GetUserDetails()
        {
            bool isAdmin = false;

            Jwt jwt = Jwt.GetJwtFromRequestHeader(Request);
            if (jwt != null)
            {
                JwtPayload jwtPayload = jwt.Payload;

                if (jwtPayload != null)
                {
                    string email = jwtPayload.Email;

                    int count = db.Administrators.Count(admin => admin.Email == email);
                    if (count > 0)
                    {
                        isAdmin = true;
                    }
                }
            }

            return new UserDetailsDTO
            {
                IsAdmin = isAdmin
            };
        }

        /// <summary>
        /// List the authenticated user borrowed books.
        /// </summary>
        [Route("books")]
        [HttpGet]
        [RaAuthentication]
        [RaLibraryAuthorize(Roles = RoleTypes.NormalUsers)]
        public IQueryable<Book> ListBorrowedBooks()
        {
            var identity = User.Identity as ClaimsIdentity;
            var email = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

            return db.Books.Where(book => book.Borrower == email);
        }

        /// <summary>
        /// Borrow a book for the authenticated user.
        /// </summary>
        /// <param name="book">The borrowed book.</param>
        [Route("books")]
        [HttpPost]
        [RaAuthentication]
        [RaLibraryAuthorize(Roles = RoleTypes.NormalUsers)]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> BorrowBook(Book book)
        {
            var identity = User.Identity as ClaimsIdentity;
            var email = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

            BorrowLog borrowLogRecord = new BorrowLog()
            {
                F_BookID = book.Id,
                Borrower = email,
                BorrowTime = DateTime.UtcNow
            };

            db.BorrowLogs.Add(borrowLogRecord);

            Book bookRecord = await db.Books.FindAsync(book.Id);
            if (bookRecord == null)
            {
                return NotFound();
            }
            else
            {
                bookRecord.Borrower = email;
                db.Entry(bookRecord).State = EntityState.Modified;
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Return a book for the authenticated user.
        /// </summary>
        /// <param name="id">The book's id.</param>
        [Route("books/{id:int}")]
        [HttpDelete]
        [RaAuthentication]
        [RaLibraryAuthorize(Roles = RoleTypes.NormalUsers)]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> ReturnBook(int id)
        {
            var logRecord = db.BorrowLogs.Where(log => log.F_BookID == id && log.ReturnTime == null).First();
            if (logRecord == null)
            {
                return NotFound();
            }
            else
            {
                logRecord.ReturnTime = DateTime.UtcNow;
                db.Entry(logRecord).State = EntityState.Modified;
            }

            var bookRecord = await db.Books.FindAsync(id);
            if (bookRecord == null)
            {
                return NotFound();
            }
            else
            {
                bookRecord.Borrower = null;
                db.Entry(bookRecord).State = EntityState.Modified;
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return StatusCode(HttpStatusCode.NoContent);
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
