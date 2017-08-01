using System;
using System.Linq;

namespace LinkShortener.API
{
    public class LinkShortenerService : ILinkShortenerService
    {
        private static readonly Random Random = new Random();

        public string CreateShortLink(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}