using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkShortener.API.Models;
using LinkShortener.API.Repository;

namespace LinkShortener.API.Services.LinkShortener.Impl
{
    public class LinkShortenerService : ILinkShortenerService
    {
        private readonly IRepository<ShortLink> _repository;
        private readonly IBasicCollisionResolverBuilder _builder;

        public LinkShortenerService(IRepository<ShortLink> repository, IBasicCollisionResolverBuilder collisionResolverBuilder)
        {
            _repository = repository;
            _builder = collisionResolverBuilder;
        }

        public async Task<string> CreateShortLinkAsync(string fullLink)
        {
            Func<string, Task<bool>> checkFunction = ShortLinkExists;
            Func<ShortLink, Task> onSuccessFunction = _repository.InsertAsync;

            var collisionResolver = _builder.WithOnSuccessFunction(onSuccessFunction)
                .WithMaximumAttemptsCount(5).WithCheckExistenceFunction(checkFunction).Build();

            return await collisionResolver.FindSuitableShortLinkAsync(fullLink);
        }

        public async Task<IEnumerable<ShortLink>> GetAllShortenedLinksAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ShortLink> GetFullLinkAsync(string shortLink)
        {
            return (await _repository.GetAllAsync()).SingleOrDefault(l => l.Key == shortLink);
        }

        private async Task<bool> ShortLinkExists(string shortLink)
        {
            return (await _repository.GetAllAsync()).Any(l => l.Key == shortLink);
        }
    }
}