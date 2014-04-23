using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.Http;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;

namespace ProblemJsonWebapi.Test
{
    public class ProblemJsonMediaTypeFormatterTest
    {
        [Test]
        public void Ctor()
        {
            var formatter = new ProblemJsonMediaTypeFormatter();

            formatter.SupportedMediaTypes.Should().BeEmpty();
            formatter.MediaTypeMappings.Should().Contain(p =>  p.GetType() == typeof(ProblemJsonMediaTypeMapping));
        }

        [Test]
        public void CanReadType()
        {
            var formatter = new ProblemJsonMediaTypeFormatter();
            formatter.CanReadType(typeof (object)).Should().BeFalse();
            formatter.CanReadType(typeof(HttpError)).Should().BeFalse();
        }

        [Test]
        public void CanWriteType()
        {
            var formatter = new ProblemJsonMediaTypeFormatter();
            formatter.CanWriteType(typeof(object)).Should().BeFalse();
            formatter.CanWriteType(typeof(HttpError)).Should().BeTrue();
        }

        [Test]
        public void WriteToStream()
        {
            var result = WriteToStreamTestHelper(new HttpError("details"));

            result.Should().Be("{\"type\":\"about:blank\",\"title\":\"details\",\"instance\":\"urn:ietf:rfc:draft-nottingham-http-problem-06:x-exception:\"}");
        }

        [Test]
        public void WriteExceptionToStream()
        {
            var result = WriteToStreamTestHelper(new HttpError(new UnauthorizedAccessException("Access denied"), true));

            result.Should().Be("{\"type\":\"about:blank\",\"title\":\"An error has occurred.\",\"detail\":\"Access denied\",\"instance\":\"urn:ietf:rfc:draft-nottingham-http-problem-06:x-exception:System.UnauthorizedAccessException\"}");
        }
        

        public string WriteToStreamTestHelper(HttpError error)
        {
            var ms = new MemoryStream();

            var fixture = new Fixture();
            fixture.Customize(new AutoNSubstituteCustomization());

            var content = fixture.Create<HttpContent>();
            var transport = fixture.Create<TransportContext>();

            var formatter = new ProblemJsonMediaTypeFormatter();

            var task = formatter.WriteToStreamAsync(error.GetType(),
              error,
              ms,
              content,
              transport, new CancellationToken(false)
            );

            task.Wait();

            ms.Seek(0, SeekOrigin.Begin);

            return (new StreamReader(ms)).ReadToEnd();
        }
    }
}
