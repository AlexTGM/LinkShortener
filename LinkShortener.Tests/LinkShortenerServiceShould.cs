using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LinkShortener.API.Models;
using LinkShortener.API.Repository;
using LinkShortener.API.Services.LinkShortener;
using LinkShortener.API.Services.LinkShortener.Impl;
using Moq;
using Xunit;

namespace LinkShortener.Tests
{
    public class LinkShortenerServiceShould
    {
        private readonly Mock<IRepository<ShortLink>> _shortLinksRepository;

        private readonly ILinkShortenerService _service;

        private readonly IEnumerable<User> _users;
        private readonly IEnumerable<ShortLink> _shortLinks;

        public LinkShortenerServiceShould()
        {
            var builder = new Mock<IBasicCollisionResolverBuilder>();

            _users = new[] { new User {UserName = "User1"}, new User { UserName = "User2"} };

            _shortLinks = new[]
            {
                new ShortLink("ABCD", "https://google.com", _users.First()),
                new ShortLink("ABCE", "https://yandex.com", _users.First()),
                new ShortLink("ABCF", "https://bing.com")
            };

            _shortLinksRepository = new Mock<IRepository<ShortLink>>();
            _shortLinksRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(_shortLinks);

            _service = new LinkShortenerService(_shortLinksRepository.Object, builder.Object);
        }

        [Fact]
        public async Task GetAllShortenedLinks()
        {
            _shortLinksRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(_shortLinks);

            var actual = await _service.GetAllShortenedLinksAsync();

            _shortLinksRepository.Verify(r => r.GetAllAsync(), Times.Once);
            
            actual.ShouldBeEquivalentTo(_shortLinks);
        }

        [Fact]
        public async Task ReturnFullLink()
        {
            var expected = _shortLinks.First();

            var actual = await _service.GetFullLinkAsync(expected.Key);

            actual.FullLink.ShouldBeEquivalentTo(expected.FullLink);
        }

        [Fact]
        public async Task ReturnNullIfShortLinkDoesNotExist()
        {
            var actual = await _service.GetFullLinkAsync(It.IsAny<string>());

            actual.ShouldBeEquivalentTo(null);
        }

        [Fact]
        public async Task ReturnShortLinksRelatedToUser()
        {
            var links = await _service.GetAllShortenedLinksRelatedToUserAsync(_users.First());

            links.ShouldBeEquivalentTo(_shortLinks.Take(2));
        }
    }
}