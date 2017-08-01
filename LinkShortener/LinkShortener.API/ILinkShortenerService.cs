namespace LinkShortener.API
{
    public interface ILinkShortenerService
    {
        string CreateShortLink(int inputLink);
    }
}