namespace LinkShortener.API.Models
{
    public class ShortLink : BaseEntity
    {
        public User User { get; set; }
        public string Key { get; set; }
        public string FullLink { get; set; }

        public ShortLink() { }

        public ShortLink(string key, string fullLink)
        {
            Key = key;
            FullLink = fullLink;
        }

        public ShortLink(string key, string fullLink, User user)
            : this(key, fullLink)
        {
            User = user;
        }
    }
}