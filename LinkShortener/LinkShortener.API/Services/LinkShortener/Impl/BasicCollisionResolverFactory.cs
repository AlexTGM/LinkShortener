using System;
using System.Threading.Tasks;

namespace LinkShortener.API.Services.LinkShortener.Impl
{
    public class BasicCollisionResolverFactory : ICollisionResolverFactory<BasicCollisionResolver>
    {
        private readonly IShortLinkGenerator _generator;

        public BasicCollisionResolverFactory(IShortLinkGenerator generator)
        {
            _generator = generator;
        }

        public BasicCollisionResolver Create(Func<string, Task<bool>> checkExistenceFunction)
        {
            return new BasicCollisionResolver(_generator, checkExistenceFunction);
        }
    }
}