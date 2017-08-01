using FluentAssertions;
using Xunit;

namespace LinkShortener.Tests
{
    public class LinkShortenerServiceShould
    {
        [Theory]
        [InlineData("http://google.com")]
        [InlineData("http://yandex.com")]
        public void CreateShortLink(string inputLink)
        {
            var linkShortenerService = new LinkShortenerService();
            var actual = linkShortenerService.CreateShortLink(inputLink);

            actual.ShouldBeEquivalentTo("http://shortlink.com");
        }
    }

    public class LinkShortenerService : ILinkShortenerService
    {
        public string CreateShortLink(string inputLink)
        {
            return "shortlink";
        }
    }

    public interface ILinkShortenerService
    {
        string CreateShortLink(string inputLink);
    }
}