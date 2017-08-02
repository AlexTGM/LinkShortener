using System;
using System.Threading.Tasks;
using LinkShortener.API.Models;

namespace LinkShortener.API.Services.LinkShortener
{
    public interface ICollisionResolver
    {
        Func<string, Task<bool>> CheckExistenceFunction { get; }
        Func<ShortLink, Task> OnSuccessFunction { get; }

        Task<string> FindSuitableShortLinkAsync(string fullLink);
    }
}