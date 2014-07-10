using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Newtonsoft.Json;

namespace ProblemJsonWebapi.Owin
{
// ReSharper disable once ClassNeverInstantiated.Global
    internal class ProblemJsonExceptionFilter : OwinMiddleware
    {
        private const string AcceptHeader = "Accept";
        private const string ProblemJsonContentType = "application/problem+json";

        private readonly bool _developerMode;

        public ProblemJsonExceptionFilter(OwinMiddleware next, bool developerMode)
            : base(next)
        {
            _developerMode = developerMode;
        }

        public override async Task Invoke(IOwinContext context)
        {
            HttpResponseException exception;

            try
            {
                await Next.Invoke(context);
                return;
            }
            catch (HttpResponseException ex)
            {
                exception = ex;
            }
            catch (Exception)
            {
                exception = new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            var owinResponse = context.Response;

            owinResponse.StatusCode = (int)exception.Response.StatusCode;

            if (!SupportProblemJson(context.Request))
                return;

            var problemJsonResponse = CreateResponse(exception);

            await WriteResponse(owinResponse, problemJsonResponse);
           
        }

        private ProblemJsonModel CreateResponse(HttpResponseException exception)
        {
            var problemJsonResponse = new ProblemJsonModel
            {
                TypeUri = "about:blank",
                Status = (int)exception.Response.StatusCode,
                Instance = string.Format("urn:ietf:rfc:draft-nottingham-http-problem-06:x-exception:{0}", exception.GetType().FullName),
            };

            if (_developerMode)
            {
                problemJsonResponse.Detail = exception.Message;
                problemJsonResponse.DebugInfo = exception.StackTrace;
                problemJsonResponse.Title = exception.Message;
            }
            return problemJsonResponse;
        }

        private static async Task WriteResponse(IOwinResponse owinResponse, ProblemJsonModel problemJsonResponse)
        {
            var message = JsonConvert.SerializeObject(problemJsonResponse);

            var bytes = Encoding.UTF8.GetBytes(message);

            owinResponse.Headers.Add("Content-type", new[] { ProblemJsonContentType, "charset=utf-8" });

            await owinResponse.Body.WriteAsync(bytes, 0, bytes.Length);
        }

        private static bool SupportProblemJson(IOwinRequest request)
        {
            if (!request.Headers.ContainsKey(AcceptHeader))
                return false;

            var headerValue = request.Headers[AcceptHeader];
            
            if (string.IsNullOrEmpty(headerValue))
                return false;

            return headerValue.IndexOf(ProblemJsonContentType, StringComparison.InvariantCultureIgnoreCase) != -1;
        }
    }
}
