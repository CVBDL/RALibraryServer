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
        public UserDetailsDTO GetUserDetails()
        {
            Jwt jwt = Jwt.GetJwtFromRequestHeader(Request);
            if (jwt != null)
            {
                JwtPayload jwtPayload = jwt.Payload;

                if (jwtPayload != null)
                {
                    string Email = jwtPayload.Email;
                }
            }

            return new UserDetailsDTO
            {
                IsAdmin = false
            };
        }

        /// <summary>
        /// List the authenticated user borrowed books.
        /// </summary>
        [Route("books")]
        [HttpGet]
        public void ListBorrowedBooks() { }

        /// <summary>
        /// Borrow a book for the authenticated user.
        /// </summary>
        /// <param name="book">The borrowed book.</param>
        [Route("books")]
        [HttpPost]
        public async Task<IHttpActionResult> BorrowBook(Book book) 
        {
            string user = "lliao2@ra.rockwell.com";
            DateTime borrowTime = DateTime.UtcNow;

            BorrowLog log = new BorrowLog();
            log.Book = book;
            log.Borrower = user;
            log.BorrowTime = borrowTime;
            log.F_BookID = book.Id;

            db.BorrowLogs.Add(log);

            await db.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Return a book for the authenticated user.
        /// </summary>
        /// <param name="id">The book's id.</param>
        [Route("books/{id:int}")]
        [HttpDelete]
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
