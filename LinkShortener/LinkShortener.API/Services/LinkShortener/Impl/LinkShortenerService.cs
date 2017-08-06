using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LinkShortener.API.Exceptions;
using LinkShortener.API.Models;
using LinkShortener.API.Models.Database;
using LinkShortener.API.Models.DTO;
using LinkShortener.API.Repository;

namespace LinkShortener.API.Services.LinkShortener.Impl
{
    public class LinkShortenerService : ILinkShortenerService
    {
        private readonly ICollisionResolver _collisionResolver;
        private readonly IRepository _shortLinksRepository;

        public LinkShortenerService(IRepository shortLinksRepository,
            ICollisionResolverFactory<ICollisionResolver> collisionResolverFactory)
        {
            _shortLinksRepository = shortLinksRepository;
            _collisionResolver = collisionResolverFactory.Create(ShortLinkExists);
        }

        public async Task<string> CreateShortLinkAsync(string fullLink, User user)
        {
            var pattern =
                "^(http:\\/\\/www\\.|https:\\/\\/www\\.|http:\\/\\/|https:\\/\\/)?[a-z0-9]+([\\-\\.]{1}[a-z0-9]+)*\\.[a-z]{2,5}(:[0-9]{1,5})?(\\/.*)?$";

            if (!Regex.IsMatch(fullLink, pattern))
                throw new InvalidLinkException();

            var shortLink = await _collisionResolver.FindSuitableShortLinkAsync();

            await _shortLinksRepository.InsertAsync(new ShortLink(shortLink, fullLink, user));

            return shortLink;
        }

        public async Task<IEnumerable<ShortLinkDto>> GetAllShortenedLinksAsync()
        {
            var allLinks = await _shortLinksRepository.GetAllAsync();
            return allLinks.Select(x => new ShortLinkDto(x));
        }

        public async Task<ShortLinkDto> GetFullLinkAsync(string shortLink)
        {
            var allLinks = await _shortLinksRepository.GetAllAsync();
            var result = allLinks.SingleOrDefault(l => l.Key == shortLink);

            if (result == null) return null;

            result.CallsCount++;
            await _shortLinksRepository.UpdateAsync();

            return new ShortLinkDto(result);
        }

        public async Task<IEnumerable<ShortLinkDto>> GetAllShortenedLinksRelatedToUserAsync(User user)
        {
            var allLinks = await _shortLinksRepository.GetAllAsync(user);
            return allLinks.Select(x => new ShortLinkDto(x));
        }

        public async Task<PaginatedData<ShortLinkDto>> GetAllShortenedLinksRelatedToUserPaginatedAsync(User user, int skip, int take)
        {
            var links = await _shortLinksRepository.GetPageAsync(skip, take, user);

            var shortLinkDtos = links.Data.Select(l => new ShortLinkDto(l));

            return new PaginatedData<ShortLinkDto>
            {
                Data = shortLinkDtos, Page = skip / take + 1,
                Size = take, TotalCount = links.TotalCount
            };
        }

        private async Task<bool> ShortLinkExists(string shortLink)
        {
            return (await _shortLinksRepository.GetAllAsync()).Any(l => l.Key == shortLink);
        }
    }
}