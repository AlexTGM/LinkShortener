using System;
using System.Threading.Tasks;
using LinkShortener.API.Models;
using LinkShortener.Tests;

namespace LinkShortener.API.Services.LinkShortener.Impl
{
    public class BasicCollisionResolver : ICollisionResolver
    {
        private readonly IShortLinkGenerator _shortLinkGenerator;

        public int MaximumAttemptsCount { get; set; }
        
        public Func<string, Task<bool>> CheckExistenceFunction { get; set; }
        public Func<ShortLink, Task> OnSuccessFunction { get; set; }

        public BasicCollisionResolver(IShortLinkGenerator shortLinkGenerator)
        {
            _shortLinkGenerator = shortLinkGenerator;
        }

        public async Task<string> FindSuitableShortLinkAsync(string fullLink)
        {
            for (var attempt = 0; attempt < MaximumAttemptsCount; attempt++)
            {
                var shortLink = _shortLinkGenerator.CreateShortLink(7);

                if (await CheckExistenceFunction(shortLink)) continue;
                if (OnSuccessFunction != null)
                    await OnSuccessFunction(new ShortLink(shortLink, fullLink));

                return shortLink;
            }

            throw new MaximumAttemptsReachedException(MaximumAttemptsCount);
        }
    }
}