using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace RaLibrary.Filters
{
    public class RaLibraryAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            // Only authenticated user is necessary to check authorization.
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return false;
            }

            // Default to checking if is "NormalUsers".
            if (string.IsNullOrWhiteSpace(Roles))
            {
                return HttpContext.Current.User.IsInRole(RoleTypes.NormalUsers);
            }
            else
            {
                bool isAuthorized = false;
                string[] roleList = Roles.Split(',');
                for (int i = 0; i < roleList.Length; i++)
                {
                    if (HttpContext.Current.User.IsInRole(roleList[i]))
                    {
                        isAuthorized = true;
                    }
                }
                return isAuthorized;
            }
        }

        public override bool AllowMultiple
        {
            get { return false; }
        }
    }
}
