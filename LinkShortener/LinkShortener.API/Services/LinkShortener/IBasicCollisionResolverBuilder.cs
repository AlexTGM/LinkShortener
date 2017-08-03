using System;
using System.Threading.Tasks;
using LinkShortener.API.Services.LinkShortener.Impl;

namespace LinkShortener.API.Services.LinkShortener
{
    public interface IBasicCollisionResolverBuilder
    {
        BasicCollisionResolver Build();
        IBasicCollisionResolverBuilder WithCheckExistenceFunction(Func<string, Task<bool>> function);
        IBasicCollisionResolverBuilder WithMaximumAttemptsCount(int attemptsCount);
    }
}