using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinimalApi.Dom.Databases.Entities;
using MinimalApi.Dom.Facilities;
using MinimalApi.Dom.Facilities.ValueObjects;

namespace MinimalApi.Infra
{
    internal class FacilityConfiguration : IEntityTypeConfiguration<Facility>
    {
        public void Configure(EntityTypeBuilder<Facility> builder)
        {
            builder.ToTable("FAC", "CMN_MSTR");

            builder.Property(p => p.Id).HasColumnName("FAC_ID")
                .HasConversion(id => id.Value, value => FacilityId.Create(value));
            builder.Property(p => p.Name).HasColumnName("NM");

            builder.HasMany(e => e.Databases).WithMany(e => e.Facilities).UsingEntity<DatabaseFacility>();
        }
    }
}
