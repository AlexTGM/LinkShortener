using System.Threading.Tasks;

namespace LinkShortener.API.Services.LinkShortener
{
    public interface ICollisionResolver
    {
        Task<string> FindSuitableShortLinkAsync(int maximimAttemptsCount = 5);
    }
}