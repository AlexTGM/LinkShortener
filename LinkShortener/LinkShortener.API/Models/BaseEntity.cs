using System;

namespace LinkShortener.API.Models
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public DateTime DateCreated { get; set; }

        public BaseEntity()
        {
            DateCreated = DateTime.Now;
        }
    }
}