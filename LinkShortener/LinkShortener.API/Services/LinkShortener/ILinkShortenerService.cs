using System.Collections.Generic;
using System.Threading.Tasks;
using LinkShortener.API.Models.Database;
using LinkShortener.API.Models.DTO;

namespace LinkShortener.API.Services.LinkShortener
{
    public interface ILinkShortenerService
    {
        Task<string> CreateShortLinkAsync(string fullLink, User user);
        Task<IEnumerable<ShortLinkDto>> GetAllShortenedLinksAsync();
        Task<ShortLinkDto> GetFullLinkAsync(string shortLink);
        Task<IEnumerable<ShortLinkDto>> GetAllShortenedLinksRelatedToUserAsync(User user);
    }
}