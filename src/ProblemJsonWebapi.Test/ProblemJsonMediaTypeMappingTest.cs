using System;
using System.Net.Http;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;

namespace ProblemJsonWebapi.Test
{
    public class ProblemJsonMediaTypeMappingTest
    {
        [Test]
        public void EmptyAcceptNoMatch()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoNSubstituteCustomization());
            
            var mapping = new ProblemJsonMediaTypeMapping();

            var request = fixture.Create<HttpRequestMessage>();

            var quality = mapping.TryMatchMediaType(request);

            quality.Should().Be(0.0);

        }

        [Test]
        public void WrongAcceptNoMatch()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoNSubstituteCustomization());

            var mapping = new ProblemJsonMediaTypeMapping();

            var request = fixture.Create<HttpRequestMessage>();
            
            request.Headers.Add("accept", "application/json");

            var quality = mapping.TryMatchMediaType(request);

            quality.Should().Be(0.0);

        }

        [Test]
        public void RightAcceptMatch()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoNSubstituteCustomization());

            var mapping = new ProblemJsonMediaTypeMapping();

            var request = fixture.Create<HttpRequestMessage>();

            request.Headers.Add("accept", new [] {"application/problem+json", "application/json"});

            var quality = mapping.TryMatchMediaType(request);

            quality.Should().Be(1.0);

        }

        [Test]
        public void RightAcceptCaseMatch()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoNSubstituteCustomization());

            var mapping = new ProblemJsonMediaTypeMapping();

            var request = fixture.Create<HttpRequestMessage>();

            request.Headers.Add("accept", new[] { "Application/Problem+JSON", "application/json" });

            var quality = mapping.TryMatchMediaType(request);

            quality.Should().Be(1.0);

        }
    }
}
