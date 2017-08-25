using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using RaLibrary.Models;
using RaLibrary.Utils;
using System.Security.Claims;
using RaLibrary.Filters;

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
        [RaAuthentication(Realm = "user")]
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

                    int count = (from admin in db.Administrators
                                 where admin.Email == email
                                 select admin).Count();

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
        [RaAuthentication(Realm = "user")]
        public IQueryable<Book> ListBorrowedBooks()
        {
            ClaimsIdentity identity = User.Identity as ClaimsIdentity;
            string email = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

            IQueryable<Book> result = from book in db.Books
                         where book.Borrower == email
                         select book;

            return result;
        }

        /// <summary>
        /// Borrow a book for the authenticated user.
        /// </summary>
        /// <param name="book">The borrowed book.</param>
        [Route("books")]
        [HttpPost]
        [RaAuthentication(Realm = "user")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> BorrowBook(Book book)
        {
            string user = "test@example.com";

            BorrowLog log = new BorrowLog();
            log.F_BookID = book.Id;
            log.Borrower = user;
            log.BorrowTime = DateTime.UtcNow;

            db.BorrowLogs.Add(log);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Return a book for the authenticated user.
        /// </summary>
        /// <param name="id">The book's id.</param>
        [Route("books/{id:int}")]
        [HttpDelete]
        [RaAuthentication(Realm = "user")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> ReturnBook(int id)
        {
            var logs = from log in db.BorrowLogs
                       where (log.F_BookID == id && log.ReturnTime == null)
                       select log;

            DateTime returnTime = DateTime.UtcNow;

            if (logs.Count() == 1)
            {
                logs.ElementAt(0).ReturnTime = returnTime;
            }
            else
            {
                return NotFound();
            }

            await db.SaveChangesAsync();

            return Ok();
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
