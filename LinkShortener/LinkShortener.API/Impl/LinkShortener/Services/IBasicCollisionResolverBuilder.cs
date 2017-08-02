using System;
using System.Threading.Tasks;
using LinkShortener.API.Models;
using LinkShortener.API.Services.LinkShortener.Impl;

namespace LinkShortener.API.Impl.LinkShortener.Services
{
    public interface IBasicCollisionResolverBuilder
    {
        BasicCollisionResolver Build();
        IBasicCollisionResolverBuilder WithCheckExistenceFunction(Func<string, Task<bool>> function);
        IBasicCollisionResolverBuilder WithMaximumAttemptsCount(int attemptsCount);
        IBasicCollisionResolverBuilder WithOnSuccessFunction(Func<ShortLink, Task> function);
    }
}