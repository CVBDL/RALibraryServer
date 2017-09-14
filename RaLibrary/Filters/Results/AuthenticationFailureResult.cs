using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RaLibrary.Filters.Results
{
    public class AuthenticationFailureResult : IHttpActionResult
    {
        public AuthenticationFailureResult(HttpRequestMessage request)
        {
            Request = request;
        }

        public HttpRequestMessage Request { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            return response;
        }
    }
}
