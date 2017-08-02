using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using LinkShortener.API.Services;
using LinkShortener.API.Services.LinkShortener;
using LinkShortener.API.Services.LinkShortener.Impl;

namespace LinkShortener.Tests
{
    public class ShortLinkGeneratorShould
    {
        private readonly IShortLinkGenerator _generator = new ShortLinkGenerator();

        [Theory]
        [InlineData("http://google.com")]
        [InlineData("http://yandex.com")]
        public void GenerateShortLink(string inputLink)
        {
            var actual = _generator.CreateShortLink(7);

            actual.Should().MatchRegex("[A-Z0-9]{7}");
        }

        [Fact]
        public void ShouldGenerateUniqueValues()
        {
            var generatedLinks = new HashSet<string>();

            for (var i = 0; i < 1000000; i++)
            {
                var link = _generator.CreateShortLink(9);
                generatedLinks.Add(link);
            }

            generatedLinks.Should().HaveCount(1000000);
        }
    }
}