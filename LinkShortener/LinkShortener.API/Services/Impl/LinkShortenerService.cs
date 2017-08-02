using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkShortener.API.Models;
using LinkShortener.API.Repository;

namespace LinkShortener.API.Services.Impl
{
    public class LinkShortenerService : ILinkShortenerService
    {
        private readonly IRepository<ShortLink> _repository;
        private readonly IShortLinkGenerator _generator;

        public LinkShortenerService(IRepository<ShortLink> repository, IShortLinkGenerator generator)
        {
            _repository = repository;
            _generator = generator;
        }

        public async Task<string> CreateShortLinkAsync(string fullLink)
        {
            while (true)
            {
                var shortLink = _generator.CreateShortLink(7);

                if (await IsShortLinkExists(shortLink)) continue;
                await _repository.InsertAsync(new ShortLink(shortLink, fullLink));

                return shortLink;
            }
        }

        public async Task<IEnumerable<ShortLink>> GetAllShortenedLinksAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ShortLink> GetFullLinkAsync(string shortLink)
        {
            return (await _repository.GetAllAsync()).SingleOrDefault(l => l.Key == shortLink);
        }

        private async Task<bool> IsShortLinkExists(string shortLink)
        {
            return (await _repository.GetAllAsync()).Any(l => l.Key == shortLink);
        }
    }
}