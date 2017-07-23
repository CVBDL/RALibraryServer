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
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private RaLibraryContext db = new RaLibraryContext();

        [Route("")]
        [HttpPost]
        public void GetAccessToken() { }

        [Route("details")]
        [HttpPost]
        public void GetUserDetails() { }

        [Route("books")]
        [HttpGet]
        public void ListBorrowedBooks() { }

        [Route("books/{id:int}")]
        [HttpDelete]
        public void ReturnBorrowedBook(int id) { }

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
