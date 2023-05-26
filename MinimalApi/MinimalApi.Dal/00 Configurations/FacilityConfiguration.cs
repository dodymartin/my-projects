using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MinimalApi.Dal
{
    internal class FacilityConfiguration : IEntityTypeConfiguration<Facility>
    {
        public void Configure(EntityTypeBuilder<Facility> builder)
        {
            builder.ToTable("FAC", "CMN_MSTR");

            builder.Property(p => p.Id).HasColumnName("FAC_ID");
            builder.Property(p => p.Name).HasColumnName("NM");

            builder.HasMany(e => e.Databases).WithMany(e => e.Facilities).UsingEntity<DatabaseFacility>();
        }
    }
}
