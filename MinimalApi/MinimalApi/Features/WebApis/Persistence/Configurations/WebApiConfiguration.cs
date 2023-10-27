using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MinimalApi.Api.Features.WebApis;

internal class WebApiConfiguration : IEntityTypeConfiguration<WebApi>
{
    public void Configure(EntityTypeBuilder<WebApi> builder)
    {
        builder.ToTable("WEB_API", "CMN_MSTR");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("WEB_API_ID")
            .HasConversion(id => id.Value, value => WebApiId.Create(value));
        builder.Property(p => p.ApplicationId).HasColumnName("APLN_ID")
            .HasConversion(id => id.Value, value => ApplicationId.Create(value));
        builder.Property(p => p.UseHttps).HasColumnName("USE_HTTPS");

        builder.Metadata
            .FindNavigation(nameof(WebApi.Controllers))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.OwnsMany(t => t.Controllers, ConfigureWebApiControllersTable);

        builder.Metadata
            .FindNavigation(nameof(WebApi.Versions))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.OwnsMany(t => t.Versions, ConfigureWebApiVersionsTable);
    }

    private static void ConfigureWebApiControllersTable(OwnedNavigationBuilder<WebApi, WebApiController> builder)
    {
        builder.ToTable("WEB_API_CTLR", "CMN_MSTR")
            .WithOwner()
            .HasForeignKey("WEB_API_ID");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("WEB_API_CTLR_ID")
            .HasConversion(id => id.Value, value => WebApiControllerId.Create(value));
        builder.Property(p => p.UriName).HasColumnName("URI_NM");
    }

    private static void ConfigureWebApiVersionsTable(OwnedNavigationBuilder<WebApi, WebApiVersion> builder)
    {
        builder.ToTable("WEB_API_VER", "CMN_MSTR");

        builder
            .WithOwner()
            .HasForeignKey("WEB_API_ID");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("WEB_API_VER_ID")
            .HasConversion(id => id.Value, value => WebApiVersionId.Create(value));
        builder.Property(p => p.Port).HasColumnName("PORT");
        builder.Property(p => p.Version).HasColumnName("VER");
    }
}
