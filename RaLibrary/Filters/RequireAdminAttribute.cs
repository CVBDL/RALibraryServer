using RaLibrary.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace RaLibrary.Filters
{
    public class RequireAdminAttribute : AuthorizeAttribute
    {
        private RaLibraryContext db = new RaLibraryContext();

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var email = string.Empty;
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;

            if (identity != null && identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email) != null)
            {
                email = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                return db.Administrators.Count(admin => admin.Email == email) > 0;
            }

            return false;
        }

        public override bool AllowMultiple
        {
            get { return false; }
        }
    }
}
