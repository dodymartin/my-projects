using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinimalApi.Core;

namespace MinimalApi.Infra
{
    internal class DatabaseFacilityConfiguration : IEntityTypeConfiguration<DatabaseFacility>
    {
        public void Configure(EntityTypeBuilder<DatabaseFacility> builder)
        {
            builder.ToTable("DB_FAC", "CMN_MSTR");

            builder.Property(p => p.DatabaseId).HasColumnName("DB_ID");
            builder.Property(p => p.FacilityId).HasColumnName("FAC_ID");
        }
}
}
