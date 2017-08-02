namespace LinkShortener.API.Services
{
    public interface IShortLinkGenerator
    {
        string CreateShortLink(int length);
    }
}