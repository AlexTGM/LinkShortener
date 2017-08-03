using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkShortener.API.Models.Database;
using LinkShortener.API.Models.DTO;
using LinkShortener.API.Repository;

namespace LinkShortener.API.Services.LinkShortener.Impl
{
    public class LinkShortenerService : ILinkShortenerService
    {
        private readonly ICollisionResolver _collisionResolver;
        private readonly IRepository<ShortLink> _shortLinksRepository;

        public LinkShortenerService(IRepository<ShortLink> shortLinksRepository,
            ICollisionResolverFactory<ICollisionResolver> collisionResolverFactory)
        {
            _shortLinksRepository = shortLinksRepository;
            _collisionResolver = collisionResolverFactory.Create(ShortLinkExists);
        }

        public async Task<string> CreateShortLinkAsync(string fullLink, User user)
        {
            var shortLink = await _collisionResolver.FindSuitableShortLinkAsync();

            await _shortLinksRepository.InsertAsync(new ShortLink(shortLink, fullLink, user));

            return shortLink;
        }

        public async Task<IEnumerable<ShortLinkDto>> GetAllShortenedLinksAsync()
        {
            return (await _shortLinksRepository.GetAllAsync()).Select(x => new ShortLinkDto(x));
        }

        public async Task<ShortLinkDto> GetFullLinkAsync(string shortLink)
        {
            return new ShortLinkDto(
                (await _shortLinksRepository.GetAllAsync()).SingleOrDefault(l => l.Key == shortLink));
        }

        public async Task<IEnumerable<ShortLinkDto>> GetAllShortenedLinksRelatedToUserAsync(User user)
        {
            return (await _shortLinksRepository.GetAllAsync()).Where(l => l.User == user)
                .Select(x => new ShortLinkDto(x));
        }

        private async Task<bool> ShortLinkExists(string shortLink)
        {
            return (await _shortLinksRepository.GetAllAsync()).Any(l => l.Key == shortLink);
        }
    }
}