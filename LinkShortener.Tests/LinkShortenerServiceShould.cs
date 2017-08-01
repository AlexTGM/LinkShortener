using Xunit;
using FluentAssertions;
using LinkShortener.API;
using System.Collections.Generic;

namespace LinkShortener.Tests
{
    public class LinkShortenerServiceShould
    {
        private readonly ILinkShortenerService _linkShortenerService;

        public LinkShortenerServiceShould()
        {
            _linkShortenerService = new LinkShortenerService();
        }

        [Theory]
        [InlineData("http://google.com")]
        [InlineData("http://yandex.com")]
        public void CreateShortLink(string inputLink)
        {
            var actual = _linkShortenerService.CreateShortLink(9);

            actual.Should().MatchRegex("[A-Z0-9]{9}");
        }

        [Fact]
        public void ShouldProduceUniqueValues()
        {
            var generatedLinks = new HashSet<string>();

            for (var i = 0; i < 1000000; i++)
            {
                var link = _linkShortenerService.CreateShortLink(9);
                generatedLinks.Add(link);
            }

            generatedLinks.Should().HaveCount(1000000);
        }
    }
}