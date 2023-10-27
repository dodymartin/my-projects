using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MinimalApi.Api.Features.Applications;

internal class FacilityConfiguration : IEntityTypeConfiguration<Facility>
{
    public void Configure(EntityTypeBuilder<Facility> builder)
    {
        builder.ToTable("FAC", "CMN_MSTR");

        builder.Property(p => p.Id).HasColumnName("FAC_ID")
            .HasConversion(id => id.Value, value => FacilityId.Create(value));
        builder.Property(p => p.Name).HasColumnName("NM");
    }
}
