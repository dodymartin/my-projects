using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinimalApi.Dom.WebApis;
using MinimalApi.Dom.WebApis.ValueObjects;
using ApplicationId = MinimalApi.Dom.Applications.ValueObjects.ApplicationId;

namespace MinimalApi.Infra
{
    internal class WebApiConfiguration : IEntityTypeConfiguration<WebApi>
    {
        public void Configure(EntityTypeBuilder<WebApi> builder)
        {
            builder.ToTable("WEB_API", "CMN_MSTR");

            builder.Property(p => p.Id).HasColumnName("WEB_API_ID")
                .HasConversion(id => id.Value, value => WebApiId.Create(value));
            builder.Property(p => p.ApplicationId).HasColumnName("APLN_ID")
                .HasConversion(id => id.Value, value => ApplicationId.Create(value));
            builder.Property(p => p.UseHttps).HasColumnName("USE_HTTPS");
        }
    }
}
