using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace RaLibrary.Controllers
{
    public abstract class RaLibraryController : ApiController
    {
        public virtual string ClaimEmail
        {
            get
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                Claim claim = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

                if (claim != null)
                {
                    return claim.Value;
                }
                else
                {
                    return null;
                }
            }
        }

        public virtual string ClaimName
        {
            get
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                Claim claim = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                if (claim != null)
                {
                    return claim.Value;
                }
                else
                {
                    return null;
                }
            }
        }

        public virtual List<string> ClaimRoles
        {
            get
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                List<string> roles = identity.Claims
                    .Where(c => c.Type == ClaimTypes.Actor)
                    .Select(c => c.Value)
                    .ToList();

                if (roles.Count > 0)
                {
                    return roles;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
