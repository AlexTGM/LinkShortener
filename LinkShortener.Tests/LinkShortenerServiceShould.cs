using System.Collections.Generic;
using FluentAssertions;
using LinkShortener.API.Impl.LinkShortener.Services;
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
        private readonly Mock<IRepository<ShortLink>> _repository;

        private readonly ILinkShortenerService _service;

        private readonly IEnumerable<ShortLink> _shortLinks = new[]
        {
            new ShortLink("ABCD", "https://yandex.com"),
            new ShortLink("ABCE", "https://google.com"),
            new ShortLink("ABCF", "https://bing.com")
        };

        public LinkShortenerServiceShould()
        {
            var collisionResolverBuilder = new Mock<IBasicCollisionResolverBuilder>();

            _repository = new Mock<IRepository<ShortLink>>();
            _repository.Setup(r => r.GetAllAsync()).ReturnsAsync(_shortLinks);

            _service = new LinkShortenerService(_repository.Object, collisionResolverBuilder.Object);
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