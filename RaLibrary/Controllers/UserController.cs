using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using RaLibrary.Models;

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
        public void BorrowBook(Book book) { }

        /// <summary>
        /// Return a book for the authenticated user.
        /// </summary>
        /// <param name="id">The book's id.</param>
        [Route("books/{id:int}")]
        [HttpDelete]
        public void ReturnBook(int id) {}

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
