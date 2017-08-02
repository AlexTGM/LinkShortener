namespace LinkShortener.API.Models
{
    public class ShortLink : BaseEntity
    {
        public string Key { get; set; }
        public string FullLink { get; set; }

        public ShortLink() { }

        public ShortLink(string key, string fullLink)
        {
            Key = key;
            FullLink = fullLink;
        }
    }
}