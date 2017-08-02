namespace LinkShortener.API.Services.LinkShortener
{
    public interface IShortLinkGenerator
    {
        string CreateShortLink(int length);
    }
}