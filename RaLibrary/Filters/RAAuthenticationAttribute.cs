using RaLibrary.Results;
using RaLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;


namespace RaLibrary.Filters
{
    public class RAAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public string Realm { get; set; }

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

            if (authorization.Scheme != "Bearer")
            {
                // No authentication was attempted (for this authentication method).
                // Do not set either Principal (which would indicate success) or ErrorResult (indicating an error).
                return;
            }

            if (String.IsNullOrEmpty(authorization.Parameter))
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }

            // Post token to Authentication Server of RA
            var values = new Dictionary<string, string>
                {
                   { "IdToken", authorization.Parameter },
                };

            var content = new FormUrlEncodedContent(values);

            var response = await httpClient.PostAsync("https://apcndaekbhost.ra-int.com/raauthentication/api/token/validate", content);

            string strEmail = null, strName = null;
            if (response.StatusCode == HttpStatusCode.NoContent) // succeeded
            {
                Jwt jwtInfo = Jwt.GetJwtFromRequestHeader(request);
                if (jwtInfo != null)
                {
                    JwtPayload jwtPayload = jwtInfo.Payload;

                    if (jwtPayload != null)
                    {
                        strEmail = jwtPayload.Email;
                        strName = jwtPayload.Name;
                    }
                }
            }

            if (strEmail == null)
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", request);
                return;
            }

            IList<Claim> claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.Name, strName)
                , new Claim(ClaimTypes.Email, strEmail)
            };

            ClaimsIdentity claimIdentity = new ClaimsIdentity(claimCollection, "Bearer");
            ClaimsPrincipal claimPrincipal = new ClaimsPrincipal(claimIdentity);
            if (claimPrincipal == null)
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult("Invalid Username or Email", request);
            }
            else
            {
                // Authentication was attempted and succeeded. Set Principal to the authenticated user.
                context.Principal = claimPrincipal;
                Thread.CurrentPrincipal = claimPrincipal;
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter;

            if (String.IsNullOrEmpty(Realm))
            {
                parameter = null;
            }
            else
            {
                // A correct implementation should verify that Realm does not contain a quote character unless properly
                // escaped (precededed by a backslash that is not itself escaped).
                parameter = "realm=\"" + Realm + "\"";
            }

            context.ChallengeWith("Basic", parameter);
        }

        public virtual bool AllowMultiple
        {
            get { return false; }
        }

        private static readonly HttpClient httpClient = new HttpClient();

    }
}