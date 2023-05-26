using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MinimalApi.Dal
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
