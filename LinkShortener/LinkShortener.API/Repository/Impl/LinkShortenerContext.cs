using LinkShortener.API.Models;
using LinkShortener.API.Repository.Maps;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.API.Repository.Impl
{
    public class LinkShortenerContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ShortLinkMap.Map(modelBuilder.Entity<ShortLink>());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite($"Data Source=link_shortener.db");
        }
    }
}