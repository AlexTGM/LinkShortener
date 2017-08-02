using System.Collections.Generic;
using System.Threading.Tasks;
using LinkShortener.API.Models;

namespace LinkShortener.API.Services.LinkShortener
{
    public interface ILinkShortenerService
    {
        Task<string> CreateShortLinkAsync(string fullLink);
        Task<IEnumerable<ShortLink>> GetAllShortenedLinksAsync();
        Task<ShortLink> GetFullLinkAsync(string shortLink);
    }
}