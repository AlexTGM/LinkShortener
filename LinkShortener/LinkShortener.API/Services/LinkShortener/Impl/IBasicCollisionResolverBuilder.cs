using System;
using System.Threading.Tasks;
using LinkShortener.API.Models;

namespace LinkShortener.API.Services.LinkShortener.Impl
{
    public interface IBasicCollisionResolverBuilder
    {
        BasicCollisionResolver Build();
        IBasicCollisionResolverBuilder WithCheckExistenceFunction(Func<string, Task<bool>> function);
        IBasicCollisionResolverBuilder WithMaximumAttemptsCount(int attemptsCount);
        IBasicCollisionResolverBuilder WithOnSuccessFunction(Func<ShortLink, Task> function);
    }
}