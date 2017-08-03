using System;
using LinkShortener.API.Models.Database;

namespace LinkShortener.API.Models.DTO
{
    public class ShortLinkDto
    {
        public ShortLinkDto(ShortLink shortLink)
        {
            Key = shortLink.Key;
            FullLink = shortLink.FullLink;
            DateCreated = shortLink.DateCreated;
        }

        public string Key { get; set; }
        public string FullLink { get; set; }
        public DateTime DateCreated { get; set; }
    }
}