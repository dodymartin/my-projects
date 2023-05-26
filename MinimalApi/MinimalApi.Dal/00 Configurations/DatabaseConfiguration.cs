using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MinimalApi.Dal
{
    internal class DatabaseConfiguration : IEntityTypeConfiguration<Database>
    {
        public void Configure(EntityTypeBuilder<Database> builder)
        {
            builder.ToTable("DB", "CMN_MSTR");

            builder.Property(p => p.Id).HasColumnName("DB_ID");
            builder.Property(p => p.EnvironmentType).HasColumnName("ENVIR_TP_ID");
            builder.Property(p => p.SchemaType).HasColumnName("DB_SCHEMA_TP_ID");
            builder.Property(p => p.Type).HasColumnName("DB_TP_ID");
            builder.Property(p => p.Name).HasColumnName("NM");
            builder.Property(p => p.ParentId).HasColumnName("PRNT_DB_ID");

            builder.HasMany(e => e.Facilities).WithMany(e => e.Databases).UsingEntity<DatabaseFacility>();
        }
    }
}
