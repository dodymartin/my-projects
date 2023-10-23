using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinimalApi.Dom.Databases;
using MinimalApi.Dom.Databases.ValueObjects;
using MinimalApi.Dom.Facilities;
using MinimalApi.Dom.Facilities.ValueObjects;

namespace MinimalApi.Infra
{
    internal class DatabaseConfiguration : IEntityTypeConfiguration<Database>
    {
        public void Configure(EntityTypeBuilder<Database> builder)
        {
            builder.ToTable("DB", "CMN_MSTR");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("DB_ID")
                .HasConversion(id => id.Value, value => DatabaseId.Create(value));
            builder.Property(p => p.EnvironmentType).HasColumnName("ENVIR_TP_ID");
            builder.Property(p => p.SchemaType).HasColumnName("DB_SCHEMA_TP_ID");
            builder.Property(p => p.Type).HasColumnName("DB_TP_ID");
            builder.Property(p => p.Name).HasColumnName("NM");
            builder.Property(p => p.ParentId).HasColumnName("PRNT_DB_ID")
                .HasConversion(id => id.Value, value => DatabaseId.Create(value));

            builder.Metadata
                .FindNavigation(nameof(Database.FacilityIds))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.OwnsMany(t => t.FacilityIds, ConfigureFacilitiesTable);
        }

        private static void ConfigureFacilitiesTable(OwnedNavigationBuilder<Database, FacilityId> builder)
        {
            builder.ToTable("DB_FAC", "CMN_MSTR")
                .WithOwner()
                .HasForeignKey("DB_ID");

            builder.Property(p => p.Value).HasColumnName("FAC_ID")
                .ValueGeneratedNever();
        }
    }
}
