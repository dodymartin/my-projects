using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinimalApi.Core;

namespace MinimalApi.Infra
{
    internal class WebApiVersionConfiguration : IEntityTypeConfiguration<WebApiVersion>
    {
        public void Configure(EntityTypeBuilder<WebApiVersion> builder)
        {
            builder.ToTable("WEB_API_VER", "CMN_MSTR");

            builder.Property(p => p.Id).HasColumnName("WEB_API_VER_ID")
                .HasConversion(id => id.Value, value => new WebApiVersionId(value));
            builder.Property(p => p.Port).HasColumnName("PORT");
            builder.Property(p => p.Version).HasColumnName("VER");
            builder.Property(p => p.WebApiId).HasColumnName("WEB_API_ID")
                .HasConversion(id => id.Value, value => new WebApiId(value));
        }
    }
}
