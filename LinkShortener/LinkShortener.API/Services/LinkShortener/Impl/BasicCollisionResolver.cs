using System;
using System.Threading.Tasks;
using LinkShortener.API.Exceptions;

namespace LinkShortener.API.Services.LinkShortener.Impl
{
    public class BasicCollisionResolver : ICollisionResolver
    {
        private readonly IShortLinkGenerator _shortLinkGenerator;
        private readonly Func<string, Task<bool>> _checkExistenceFunction;

        public BasicCollisionResolver(IShortLinkGenerator shortLinkGenerator,
            Func<string, Task<bool>> checkExistenceFunction = null)
        {
            _shortLinkGenerator = shortLinkGenerator;
            _checkExistenceFunction = checkExistenceFunction ?? (str => Task.Factory.StartNew(() => false));
        }

        public async Task<string> FindSuitableShortLinkAsync(int maximimAttemptsCount = 5)
        {
            for (var attempt = 0; attempt < maximimAttemptsCount; attempt++)
            {
                var shortLink = _shortLinkGenerator.CreateShortLink(7);

                if (await _checkExistenceFunction(shortLink)) continue;

                return shortLink;
            }

            throw new MaximumAttemptsReachedException(maximimAttemptsCount);
        }
    }
}