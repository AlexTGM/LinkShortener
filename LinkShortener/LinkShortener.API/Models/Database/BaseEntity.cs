using System;

namespace LinkShortener.API.Models.Database
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            DateCreated = DateTime.Now;
        }

        public long Id { get; set; }
        public DateTime DateCreated { get; set; }
    }
}