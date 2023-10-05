using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinimalApi.Core;

namespace MinimalApi.Infra
{
    internal class WebApiConfiguration : IEntityTypeConfiguration<WebApi>
    {
        public void Configure(EntityTypeBuilder<WebApi> builder)
        {
            builder.ToTable("WEB_API", "CMN_MSTR");

            builder.Property(p => p.Id).HasColumnName("WEB_API_ID")
                .HasConversion(id => id.Value, value => new WebApiId(value));
            builder.Property(p => p.ApplicationId).HasColumnName("APLN_ID")
                .HasConversion(id => id.Value, value => new Core.ApplicationId(value));
            builder.Property(p => p.UseHttps).HasColumnName("USE_HTTPS");
        }
    }
}
