using System;
using System.Threading.Tasks;

namespace LinkShortener.API.Services.LinkShortener
{
    public interface ICollisionResolverFactory<out T> where T : ICollisionResolver
    {
        T Create(Func<string, Task<bool>> checkExistenceFunction);
    }
}