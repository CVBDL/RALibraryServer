using RaLibrary.Models;
using RaLibrary.Results;
using RaLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;


namespace RaLibrary.Filters
{
    public class RaAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        private RaLibraryContext db = new RaLibraryContext();

        private string realm = "ralibrary_resources";

        public string Realm
        {
            get
            {
                return realm;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    realm = value;
                }
            }
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            if (authorization == null)
            {
                // No authentication was attempted (for this authentication method).
                // Do not set either Principal (which would indicate success) or ErrorResult (indicating an error).
                return;
            }

            if (authorization.Scheme != SCHEME)
            {
                // No authentication was attempted (for this authentication method).
                // Do not set either Principal (which would indicate success) or ErrorResult (indicating an error).
                return;
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult(request);
                return;
            }

            // Post token to Authentication Server of RA
            var values = new Dictionary<string, string>
                {
                   { "IdToken", authorization.Parameter },
                };

            var content = new FormUrlEncodedContent(values);

            var tokenValidationEndpoint = ConfigurationManager.AppSettings.Get("TokenValidationEndpoint");
            var response = await httpClient.PostAsync(tokenValidationEndpoint, content);

            string email = string.Empty;
            string name = string.Empty;

            if (response.StatusCode == HttpStatusCode.NoContent) // succeeded
            {
                Jwt jwtInfo = Jwt.GetJwtFromRequestHeader(request);
                if (jwtInfo != null)
                {
                    JwtPayload jwtPayload = jwtInfo.Payload;

                    if (jwtPayload != null)
                    {
                        email = jwtPayload.Email;
                        name = jwtPayload.Name;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult(request);
                return;
            }

            IList<Claim> claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, email)
            };

            var isAdmin = db.Administrators.Count(admin => admin.Email == email) > 0;
            if (isAdmin)
            {
                claimCollection.Add(new Claim(ClaimTypes.Role, RoleTypes.Administrators));
                claimCollection.Add(new Claim(ClaimTypes.Role, RoleTypes.NormalUsers));
            }
            else
            {
                claimCollection.Add(new Claim(ClaimTypes.Role, RoleTypes.NormalUsers));
            }

            ClaimsIdentity claimIdentity = new ClaimsIdentity(claimCollection, SCHEME);
            ClaimsPrincipal principal = new ClaimsPrincipal(claimIdentity);
            if (principal == null)
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult(request);
            }
            else
            {
                // Authentication was attempted and succeeded. Set Principal to the authenticated user.
                context.Principal = principal;
            }
        }

        /// <summary>
        /// Only add challenge to unauthorized requests.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            string error = "invalid_token";

            // A correct implementation should verify that Realm does not contain a quote character unless properly
            // escaped (precededed by a backslash that is not itself escaped).
            string parameter = string.Format("realm=\"{0}\",error=\"{1}\"", Realm, error);

            var challenge = new AuthenticationHeaderValue(SCHEME, parameter);

            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
            
            return Task.FromResult(0);
        }
        
        public virtual bool AllowMultiple
        {
            get { return false; }
        }

        private static readonly string SCHEME = "Bearer";

        private static readonly HttpClient httpClient = new HttpClient();
    }
}
