using System.Collections.Generic;
using FluentAssertions;
using LinkShortener.API.Models;
using LinkShortener.API.Repository;
using LinkShortener.API.Services;
using LinkShortener.API.Services.Impl;
using Moq;
using Xunit;

namespace LinkShortener.Tests
{
    public class LinkShortenerServiceShould
    {
        private readonly Mock<IRepository<ShortLink>> _repository;
        private readonly Mock<IShortLinkGenerator> _shortLinkGenerator;

        private readonly ILinkShortenerService _service;

        private readonly IEnumerable<ShortLink> _shortLinks = new[]
        {
            new ShortLink("ABCD", "https://yandex.com"),
            new ShortLink("ABCE", "https://google.com"),
            new ShortLink("ABCF", "https://bing.com")
        };

        public LinkShortenerServiceShould()
        {
            _shortLinkGenerator = new Mock<IShortLinkGenerator>();

            _repository = new Mock<IRepository<ShortLink>>();
            _repository.Setup(r => r.GetAllAsync()).ReturnsAsync(_shortLinks);

            _service = new LinkShortenerService(_repository.Object, _shortLinkGenerator.Object);
        }

        [Fact]
        public async void ProduceUniqueShortLinks()
        {
            var currentIndex = 0;
            var shortLinks = new [] { "ABCD", "ABCE", "ABCDEF" };

            _shortLinkGenerator.Setup(s => s.CreateShortLink(It.IsAny<int>()))
                .Returns(() => shortLinks[currentIndex++]);

            var actual = await _service.CreateShortLinkAsync(It.IsAny<string>());

            actual.ShouldBeEquivalentTo(shortLinks[2]);
        }

        [Fact]
        public async void CreateShortLink()
        {
            _shortLinkGenerator.Setup(s => s.CreateShortLink(It.IsAny<int>())).Returns("ABCDEF");

            var actual = await _service.CreateShortLinkAsync(It.IsAny<string>());

            _shortLinkGenerator.Verify(s => s.CreateShortLink(It.IsAny<int>()), Times.Once);
            _repository.Verify(r => r.InsertAsync(It.IsAny<ShortLink>()), Times.Once);

            actual.ShouldBeEquivalentTo("ABCDEF");
        }

        [Fact]
        public async void GetAllShortenedLinks()
        {
            IEnumerable<ShortLink> expectation = new[] { new ShortLink("", "") };

            _repository.Setup(r => r.GetAllAsync()).ReturnsAsync(expectation);

            var actual = await _service.GetAllShortenedLinksAsync();

            _repository.Verify(r => r.GetAllAsync(), Times.Once);
            
            actual.ShouldBeEquivalentTo(expectation);
        }

        [Fact]
        public async void ReturnFullLink()
        {
            const string excpectation = "https://google.com";

            var actual = await _service.GetFullLinkAsync("ABCE");

            actual.FullLink.ShouldBeEquivalentTo(excpectation);
        }

        [Fact]
        public async void ReturnNullIfShortLinkDoesNotExist()
        {
            var actual = await _service.GetFullLinkAsync("AEFC");

            actual.ShouldBeEquivalentTo(null);
        }
    }
}