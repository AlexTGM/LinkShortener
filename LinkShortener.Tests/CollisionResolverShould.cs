using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LinkShortener.API.Exceptions;
using LinkShortener.API.Models;
using LinkShortener.API.Services.LinkShortener;
using LinkShortener.API.Services.LinkShortener.Impl;
using Moq;
using Xunit;

namespace LinkShortener.Tests
{
    public class CollisionResolverShould
    {
        private readonly Mock<IShortLinkGenerator> _shortLinkGenerator;
        private readonly Mock<Func<string, Task<bool>>> _checkExistenceFunction;

        private readonly ICollisionResolver _collisionResolver;

        public CollisionResolverShould()
        {
            var onSuccessFunction = new Mock<Func<ShortLink, Task<string>>>();

            _shortLinkGenerator = new Mock<IShortLinkGenerator>();
            _checkExistenceFunction = new Mock<Func<string, Task<bool>>>();

            _collisionResolver = new BasicCollisionResolverBuilder(_shortLinkGenerator.Object)
                .WithMaximumAttemptsCount(5).WithCheckExistenceFunction(_checkExistenceFunction.Object)
                .WithOnSuccessFunction(onSuccessFunction.Object).Build();
        }

        [Fact]
        public async void GenerateLinkIfNoCollisionsFound()
        {
            _shortLinkGenerator.Setup(f => f.CreateShortLink(It.IsAny<int>())).Returns("ABCD");
            _checkExistenceFunction.Setup(f => f(It.IsAny<string>())).ReturnsAsync(false);

            var actual = await _collisionResolver.FindSuitableShortLinkAsync(It.IsAny<string>());

            actual.ShouldBeEquivalentTo("ABCD");
        }

        [Fact]
        public async void GenerateLinkIfCollisionsExist()
        {
            var currentAttemp = 0;
            var existedShortLinks = new[] { "ABCD", "ABCE", "ABCF", "ABCR" };
            var productedShortLinks = new[] { "ABCD", "ABCE", "ABCF", "QWERTY" };

            _shortLinkGenerator.Setup(f => f.CreateShortLink(It.IsAny<int>())).Returns(() => productedShortLinks[currentAttemp]);
            _checkExistenceFunction.Setup(f => f(It.IsAny<string>())).ReturnsAsync(() => existedShortLinks.Contains(productedShortLinks[currentAttemp++]));
            
            var actual = await _collisionResolver.FindSuitableShortLinkAsync(It.IsAny<string>());

            actual.ShouldBeEquivalentTo(productedShortLinks.Last());
        }

        [Fact]
        public void StopGeneratingLinkAfterFiveAttempts()
        {
            var currentAttemp = 0;
            var existedShortLinks = new[] { "ABCD", "ABCE", "ABCF", "ABCR", "QWERTY", "YTREWQ" };
            var productedShortLinks = new[] { "ABCD", "ABCE", "ABCF", "ABCR", "QWERTY", "YTREWQ" };

            _shortLinkGenerator.Setup(f => f.CreateShortLink(It.IsAny<int>())).Returns(() => productedShortLinks[currentAttemp]);
            _checkExistenceFunction.Setup(f => f(It.IsAny<string>())).ReturnsAsync(() => existedShortLinks.Contains(productedShortLinks[currentAttemp++]));

            Func<Task> function = () => _collisionResolver.FindSuitableShortLinkAsync(It.IsAny<string>());

            function.ShouldThrowExactly<MaximumAttemptsReachedException>();
        }
    }
}