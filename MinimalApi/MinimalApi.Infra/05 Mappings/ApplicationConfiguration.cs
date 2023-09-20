using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinimalApi.Core;

namespace MinimalApi.Infra;

internal class ApplicationConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {        
        builder.ToTable("APLN", "CMN_MSTR");

        builder.Property(p => p.Id).HasColumnName("APLN_ID");
        builder.Property(p => p.ExeName).HasColumnName("EXE_NM");
        builder.Property(p => p.FromDirectoryName).HasColumnName("FROM_DIR_NM");
        builder.Property(p => p.Name).HasColumnName("NM");
        builder.Property(p => p.MinimumAssemblyVersion).HasColumnName("MIN_ASMBLY_VER");

        builder.HasMany(e => e.Versions).WithOne();
    }
}
