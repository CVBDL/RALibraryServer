using RaLibrary.Filters.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using RaLibrary.Data.Managers;

namespace RaLibrary.Filters
{
    public class RaLibraryAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        private static readonly string Scheme = "Basic";

        private string realm = "ralibrary_resources";
        private ServiceAccountManager serviceAccounts = new ServiceAccountManager();

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

        public bool AllowMultiple
        {
            get
            {
                return false;
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

            if (authorization.Scheme != Scheme)
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

            Tuple<string, string> usernameAndPasword = ExtractUsernameAndPassword(authorization.Parameter);
            if (usernameAndPasword == null)
            {
                context.ErrorResult = new AuthenticationFailureResult(request);
            }

            string username = usernameAndPasword.Item1;
            string password = usernameAndPasword.Item2;

            if (!IdentifyAsync(username, password))
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult(request);
                return;
            }

            IList<Claim> claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, RoleTypes.ServiceAccount)
            };

            var claimIdentity = new ClaimsIdentity(claimCollection, Scheme);
            var principal = new ClaimsPrincipal(claimIdentity);

            if (principal == null)
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult(request);
                return;
            }
            else
            {
                // Authentication was attempted and succeeded. Set Principal to the authenticated user.
                context.Principal = principal;
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var error = "invalid_credential";

            // A correct implementation should verify that Realm does not contain a quote character unless properly
            // escaped (precededed by a backslash that is not itself escaped).
            var parameter = string.Format("realm=\"{0}\",error=\"{1}\"", Realm, error);
            var challenge = new AuthenticationHeaderValue(Scheme, parameter);

            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);

            return Task.FromResult(0);
        }

        private static Tuple<string, string> ExtractUsernameAndPassword(string authorizationParameter)
        {
            byte[] credentialBytes;

            try
            {
                credentialBytes = Convert.FromBase64String(authorizationParameter);
            }
            catch (FormatException)
            {
                return null;
            }

            // The currently approved HTTP 1.1 specification says characters here are ISO-8859-1.
            // However, the current draft updated specification for HTTP 1.1 indicates this encoding is infrequently
            // used in practice and defines behavior only for ASCII.
            Encoding encoding = Encoding.ASCII;
            // Make a writable copy of the encoding to enable setting a decoder fallback.
            encoding = (Encoding)encoding.Clone();
            // Fail on invalid bytes rather than silently replacing and continuing.
            encoding.DecoderFallback = DecoderFallback.ExceptionFallback;
            string decodedCredentials;

            try
            {
                decodedCredentials = encoding.GetString(credentialBytes);
            }
            catch (DecoderFallbackException)
            {
                return null;
            }

            if (String.IsNullOrEmpty(decodedCredentials))
            {
                return null;
            }

            int colonIndex = decodedCredentials.IndexOf(':');

            if (colonIndex == -1)
            {
                return null;
            }

            string userName = decodedCredentials.Substring(0, colonIndex);
            string password = decodedCredentials.Substring(colonIndex + 1);
            return new Tuple<string, string>(userName, password);
        }

        private bool IdentifyAsync(string username, string password)
        {
            return serviceAccounts.IsValidServiceAccount(username, password);
        }
    }
}
