using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinimalApi.Core;

namespace MinimalApi.Infra
{
    internal class DatabaseConfiguration : IEntityTypeConfiguration<Database>
    {
        public void Configure(EntityTypeBuilder<Database> builder)
        {
            builder.ToTable("DB", "CMN_MSTR");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("DB_ID")
                .HasConversion(id => id.Value, value => new DatabaseId(value));
            builder.Property(p => p.EnvironmentType).HasColumnName("ENVIR_TP_ID");
            builder.Property(p => p.SchemaType).HasColumnName("DB_SCHEMA_TP_ID");
            builder.Property(p => p.Type).HasColumnName("DB_TP_ID");
            builder.Property(p => p.Name).HasColumnName("NM");
            builder.Property(p => p.ParentId).HasColumnName("PRNT_DB_ID")
                .HasConversion(id => id.Value, value => new DatabaseId(value));

            builder.HasMany(e => e.Facilities).WithMany(e => e.Databases).UsingEntity<DatabaseFacility>();
        }
    }
}
