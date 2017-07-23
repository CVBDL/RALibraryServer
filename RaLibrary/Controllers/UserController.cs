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
        /// Get an access token.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public AuthorizationDTO GetAccessToken(CredentialDTO credential) {
            string userName = credential.UserName;
            string password = credential.Password;

            return new AuthorizationDTO
            {
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWV9.TJVA95OrM7E2cBab30RMHrHDcEfxjoYZgeFONFh7HgQ",
                IsAdmin = true
            };
        }

        /// <summary>
        /// Get user details.
        /// </summary>
        [Route("details")]
        [HttpPost]
        public void GetUserDetails() { }

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
