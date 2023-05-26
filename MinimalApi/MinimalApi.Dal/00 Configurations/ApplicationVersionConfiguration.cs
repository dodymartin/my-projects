using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MinimalApi.Dal
{
    internal class ApplicationVersionConfiguration : IEntityTypeConfiguration<ApplicationVersion>
    {
        public void Configure(EntityTypeBuilder<ApplicationVersion> builder)
        {
            builder.ToTable("APLN_VER", "CMN_MSTR");

            builder.Property(p => p.Id).HasColumnName("APLN_VER_ID");
            builder.Property(p => p.ApplicationId).HasColumnName("APLN_ID");
            builder.Property(p => p.FromDirectoryName).HasColumnName("FROM_DIR_NM");
            builder.Property(p => p.Version).HasColumnName("VER");
        }
    }
}
