using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace RaLibrary.Filters
{
    public class RaLibraryAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (string.IsNullOrWhiteSpace(Roles))
            {
                return HttpContext.Current.User.IsInRole(RoleTypes.NormalUsers);
            }
            else
            {
                return HttpContext.Current.User.IsInRole(Roles);
            }
        }

        public override bool AllowMultiple
        {
            get { return false; }
        }
    }
}
