using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using ProblemJsonWebapi;

// ReSharper disable once CheckNamespace
namespace System.Net.Http.Formatting
{
    public class ProblemJsonMediaTypeFormatter : JsonMediaTypeFormatter
    {
        private const string ProblemJsonMediaType = "application/problem+json";

        public ProblemJsonMediaTypeFormatter()
        {
            SupportedMediaTypes.Clear();
            MediaTypeMappings.Add(new ProblemJsonMediaTypeMapping());
        }

        public override void SetDefaultContentHeaders(Type type,
            HttpContentHeaders headers,
            MediaTypeHeaderValue mediaType)
        
        {
            headers.ContentType = new MediaTypeHeaderValue(ProblemJsonMediaType);
            base.SetDefaultContentHeaders(type, headers, headers.ContentType);
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof (HttpError);
        }

        public override Task WriteToStreamAsync(Type type,
            object value,
            Stream writeStream,
            HttpContent content,
            TransportContext transportContext,
            CancellationToken cancellationToken)
        {
            var httpError = value as HttpError;

            if (httpError == null)
                throw new ArgumentException("value is not HttpError instance");

            var problemJsonResponse = new ProblemJsonModel()
            {
                TypeUri = "about:blank",
                Detail = httpError.ExceptionMessage,
                Instance = string.Format("urn:ietf:rfc:draft-nottingham-http-problem-06:x-exception:{0}", httpError.ExceptionType),
                DebugInfo = httpError.StackTrace,
                Title = httpError.Message
            };

            return base.WriteToStreamAsync(typeof(ProblemJsonModel), problemJsonResponse, writeStream, content, transportContext, cancellationToken);            
        }
        
    }
}
