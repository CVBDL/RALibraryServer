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
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE0ODMyMDAwMDAwMDAsImVtYWlsIjoicGF0cmljay56aG9uZ0BleGFtcGxlLmNvbSJ9.jIBK2wO6qtoAdT4v5bGaPP_ytZfIMqW_4Ofh9UTLqj4",
                IsAdmin = true
            };
        }

        /// <summary>
        /// Get user details.
        /// </summary>
        /// <returns></returns>
        [Route("details")]
        [HttpPost]
        public UserDetailsDTO GetUserDetails(CredentialDTO credential)
        {
            string userName = credential.UserName;
            string password = credential.Password;

            return new UserDetailsDTO
            {
                DisplayName = "Patrick Zhong",
                EmailAddress = "patrick.zhong@example.com"
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
