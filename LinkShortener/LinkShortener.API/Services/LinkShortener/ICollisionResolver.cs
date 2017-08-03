using System;
using System.Threading.Tasks;

namespace LinkShortener.API.Services.LinkShortener
{
    public interface ICollisionResolver
    {
        Func<string, Task<bool>> CheckExistenceFunction { get; }

        Task<string> FindSuitableShortLinkAsync();
    }
}