using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinimalApi.Dom.Applications;
using MinimalApi.Dom.Applications.Entities;
using MinimalApi.Dom.Applications.ValueObjects;
using MinimalApi.Dom.Facilities.ValueObjects;
using ApplicationId = MinimalApi.Dom.Applications.ValueObjects.ApplicationId;

namespace MinimalApi.Infra;

internal class ApplicationConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.ToTable("APLN", "CMN_MSTR");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("APLN_ID")
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => ApplicationId.Create(value));
        builder.Property(p => p.ExeName).HasColumnName("EXE_NM");
        builder.Property(p => p.FromDirectoryName).HasColumnName("FROM_DIR_NM");
        builder.Property(p => p.Name).HasColumnName("NM");
        builder.Property(p => p.MinimumAssemblyVersion).HasColumnName("MIN_ASMBLY_VER");

        builder.Metadata
            .FindNavigation(nameof(Application.Facilities))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.OwnsMany(t => t.Facilities, ConfigureApplicationFacilitiesTable);

        builder.Metadata
            .FindNavigation(nameof(Application.Versions))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.OwnsMany(t => t.Versions, ConfigureApplicationVersionsTable);
    }

    private static void ConfigureApplicationFacilitiesTable(OwnedNavigationBuilder<Application, ApplicationFacility> builder)
    {
        builder.ToTable("APLN_FAC", "CMN_MSTR")
            .WithOwner("APLN_ID");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("APLN_FAC_ID")
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => ApplicationFacilityId.Create(value));
        builder.Property(p => p.FacilityId).HasColumnName("FAC_ID")
            .HasConversion(id => id.Value, value => FacilityId.Create(value));
        builder.Property(p => p.MinimumAssemblyVersion).HasColumnName("MIN_ASMBLY_VER");
    }

    private static void ConfigureApplicationVersionsTable(OwnedNavigationBuilder<Application, ApplicationVersion> builder)
    {
        builder.ToTable("APLN_VER", "CMN_MSTR")
            .WithOwner("APLN_ID");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("APLN_VER_ID")
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => ApplicationVersionId.Create(value));
        builder.Property(p => p.FromDirectoryName).HasColumnName("FROM_DIR_NM");
        builder.Property(p => p.Version).HasColumnName("VER");
    }
}
