using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkShortener.API.Models;
using LinkShortener.API.Repository;

namespace LinkShortener.API.Services.LinkShortener.Impl
{
    public class LinkShortenerService : ILinkShortenerService
    {
        private readonly IRepository<ShortLink> _shortLinksRepository;
        private readonly IBasicCollisionResolverBuilder _builder;

        public LinkShortenerService(IRepository<ShortLink> shortLinksRepository, IBasicCollisionResolverBuilder collisionResolverBuilder)
        {
            _shortLinksRepository = shortLinksRepository;
            _builder = collisionResolverBuilder;
        }

        public async Task<string> CreateShortLinkAsync(string fullLink, User user)
        {
            var collisionResolver = _builder.WithMaximumAttemptsCount(5).WithCheckExistenceFunction(ShortLinkExists).Build();

            var shortLink = await collisionResolver.FindSuitableShortLinkAsync();

            await _shortLinksRepository.InsertAsync(new ShortLink(shortLink, fullLink) {User = user});

            return shortLink;
        }

        public async Task<IEnumerable<ShortLink>> GetAllShortenedLinksAsync()
            => await _shortLinksRepository.GetAllAsync();

        public async Task<ShortLink> GetFullLinkAsync(string shortLink)
            => (await _shortLinksRepository.GetAllAsync()).SingleOrDefault(l => l.Key == shortLink);

        public async Task<IEnumerable<ShortLink>> GetAllShortenedLinksRelatedToUserAsync(User user)
            => (await _shortLinksRepository.GetAllAsync()).Where(l => l.User == user);

        private async Task<bool> ShortLinkExists(string shortLink)
            => (await _shortLinksRepository.GetAllAsync()).Any(l => l.Key == shortLink);
    }
}