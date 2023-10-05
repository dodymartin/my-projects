using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinimalApi.Core;

namespace MinimalApi.Infra
{
    internal class ApplicationVersionConfiguration : IEntityTypeConfiguration<ApplicationVersion>
    {
        public void Configure(EntityTypeBuilder<ApplicationVersion> builder)
        {
            builder.ToTable("APLN_VER", "CMN_MSTR");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("APLN_VER_ID")
                .HasConversion(id => id.Value, value => new ApplicationVersionId(value));
            builder.Property(p => p.ApplicationId).HasColumnName("APLN_ID")
                .HasConversion(id => id.Value, value => new Core.ApplicationId(value));
            builder.Property(p => p.FromDirectoryName).HasColumnName("FROM_DIR_NM");
            builder.Property(p => p.Version).HasColumnName("VER");
        }
    }
}
