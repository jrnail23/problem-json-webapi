using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace ProblemJsonWebapi.WebAPI
{
    public class ProblemJsonMediaTypeMapping : MediaTypeMapping
    {
        public ProblemJsonMediaTypeMapping() : base(new MediaTypeHeaderValue("application/problem+json")) { }

        public override double TryMatchMediaType(HttpRequestMessage request)
        {
            var any =
                request.Headers.Accept.Any(
                    h => h.MediaType.Equals(MediaType.MediaType, StringComparison.InvariantCultureIgnoreCase));

            return any ? 1.0 : 0.0;

        }
    }
}
