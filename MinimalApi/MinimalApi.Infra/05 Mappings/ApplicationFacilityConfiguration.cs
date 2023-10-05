using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinimalApi.Core;

namespace MinimalApi.Infra
{
    internal class ApplicationFacilityConfiguration : IEntityTypeConfiguration<ApplicationFacility>
    {
        public void Configure(EntityTypeBuilder<ApplicationFacility> builder)
        {
            builder.ToTable("APLN_FAC", "CMN_MSTR");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("APLN_FAC_ID")
                .HasConversion(id => id.Value, value => new ApplicationFacilityId(value));
            builder.Property(p => p.ApplicationId).HasColumnName("APLN_ID")
                .HasConversion(id => id.Value, value => new Core.ApplicationId(value));
            builder.Property(p => p.FacilityId).HasColumnName("FAC_ID")
                .HasConversion(id => id.Value, value => new FacilityId(value));
            builder.Property(p => p.MinimumAssemblyVersion).HasColumnName("MIN_ASMBLY_VER");
        }
    }
}
