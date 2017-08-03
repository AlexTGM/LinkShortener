using LinkShortener.API.Models;
using LinkShortener.API.Models.Database;
using LinkShortener.API.Repository.Maps;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.API.Repository.Impl
{
    public class LinkShortenerContext : IdentityDbContext<User>
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ShortLinkMap.Map(modelBuilder.Entity<ShortLink>());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=link_shorneter;Trusted_Connection=True;");
        }
    }
}