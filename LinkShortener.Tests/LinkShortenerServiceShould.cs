using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LinkShortener.API.Exceptions;
using LinkShortener.API.Models.Database;
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

        private readonly IList<User> _users;
        private readonly IList<ShortLink> _shortLinks;

        public LinkShortenerServiceShould()
        {
            _users = new[] { new User {UserName = "User1"}, new User { UserName = "User2"} };

            _shortLinks = new List<ShortLink>
            {
                new ShortLink("ABCD", "https://google.com", _users.First()) {CallsCount = 0},
                new ShortLink("ABCE", "https://yandex.com", _users.First()) {CallsCount = 0},
                new ShortLink("ABCF", "https://bing.com") {CallsCount = 10}
            };

            _shortLinksRepository = new Mock<IRepository<ShortLink>>();
            _shortLinksRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(_shortLinks);

            var collisionResolver = new Mock<ICollisionResolver>();
            collisionResolver.Setup(c => c.FindSuitableShortLinkAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<string>());

            var factory = new Mock<ICollisionResolverFactory<ICollisionResolver>>();
            factory.Setup(f => f.Create(It.IsAny<Func<string, Task<bool>>>())).Returns(collisionResolver.Object);

            _service = new LinkShortenerService(_shortLinksRepository.Object, factory.Object);
        }

        [Theory]
        [InlineData("QWERTY", "https://yahoo.com", "admin")]
        [InlineData("QWERTY", "https://yahoo.com", null)]
        public async Task CreateShortLink(string shortLink, string fullLink, string userName)
        {
            var user = string.IsNullOrEmpty(userName) ? null : new User(userName);
            var expected = new ShortLink("QWERTY", "https://yahoo.com", user);

            _shortLinksRepository.Setup(r => r.InsertAsync(It.IsAny<ShortLink>()))
                .Returns(() => Task.Factory.StartNew(() => _shortLinks.Add(expected)));

            await _service.CreateShortLinkAsync("https://yahoo.com", It.IsAny<User>());

            (await _shortLinksRepository.Object.GetAllAsync()).Should().Contain(expected);
        }

        [Theory]
        [InlineData("https://www.google.com/")]
        [InlineData("http://www.google.com")]
        [InlineData("www.google.com")]
        public void WorkWithValidLinks(string fullLink)
        {
            Func<Task> function = async () => await _service.CreateShortLinkAsync(fullLink, It.IsAny<User>());

            function.ShouldNotThrow();
        }

        [Theory]
        [InlineData("htt://www.google.com")]
        [InlineData("://www.google.com")]
        public void ShouldThrowErrorIfInvalidLink(string fullLink)
        {
            Func<Task> function = async () => await _service.CreateShortLinkAsync(fullLink, It.IsAny<User>());

            function.ShouldThrowExactly<InvalidLinkException>();
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

        [Fact]
        public async Task RaiseCallsCountWhenFullLinkRequested()
        {
            const int expected = 10;

            for (var i = 0; i < expected; i++)
                await _service.GetFullLinkAsync("ABCD");

            _shortLinks.First().CallsCount.ShouldBeEquivalentTo(expected);

            _shortLinksRepository.Verify(r => r.UpdateAsync(), Times.Exactly(expected));
        }
    }
}