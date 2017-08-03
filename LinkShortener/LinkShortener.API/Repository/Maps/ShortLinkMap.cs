using LinkShortener.API.Models.Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkShortener.API.Repository.Maps
{
    public static class ShortLinkMap
    {
        public static void Map(EntityTypeBuilder<ShortLink> entityBuilder)
        {
            entityBuilder.HasIndex(t => t.Id);
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.FullLink).IsRequired();
            entityBuilder.Property(t => t.DateCreated).IsRequired();
            entityBuilder.Property(t => t.Key).IsRequired();
        }
    }
}