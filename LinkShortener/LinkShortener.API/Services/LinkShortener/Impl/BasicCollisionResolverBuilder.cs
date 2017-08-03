using System;
using System.Threading.Tasks;

namespace LinkShortener.API.Services.LinkShortener.Impl
{
    public class BasicCollisionResolverBuilder : IBasicCollisionResolverBuilder
    {
        private int _attemptsCount = 1;

        private readonly IShortLinkGenerator _shortLinkGenerator;
        
        private Func<string, Task<bool>> _checkFunction;

        public BasicCollisionResolverBuilder(IShortLinkGenerator shortLinkGenerator)
        {
            _shortLinkGenerator = shortLinkGenerator;
        }

        public IBasicCollisionResolverBuilder WithMaximumAttemptsCount(int attemptsCount)
        {
            _attemptsCount = attemptsCount;
            return this;
        }

        public IBasicCollisionResolverBuilder WithCheckExistenceFunction(Func<string, Task<bool>> function)
        {
            _checkFunction = function;
            return this;
        }

        public BasicCollisionResolver Build()
        {
            var collisionResolver =
                new BasicCollisionResolver(_shortLinkGenerator)
                {
                    CheckExistenceFunction = _checkFunction ?? (async str => await Task.Factory.StartNew(() => false)),
                    MaximumAttemptsCount = _attemptsCount
                };

            return collisionResolver;
        }
    }
}