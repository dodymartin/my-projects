using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MinimalApi.Dal
{
    internal class ApplicationFacilityConfiguration : IEntityTypeConfiguration<ApplicationFacility>
    {
        public void Configure(EntityTypeBuilder<ApplicationFacility> builder)
        {
            builder.ToTable("APLN_FAC", "CMN_MSTR");

            builder.Property(p => p.Id).HasColumnName("APLN_FAC_ID");
            builder.Property(p => p.ApplicationId).HasColumnName("APLN_ID");
            builder.Property(p => p.FacilityId).HasColumnName("FAC_ID");
            builder.Property(p => p.MinimumAssemblyVersion).HasColumnName("MIN_ASMBLY_VER");
        }
    }
}
