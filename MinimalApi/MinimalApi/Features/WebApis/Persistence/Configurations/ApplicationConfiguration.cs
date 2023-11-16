using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MinimalApi.Api.Features.WebApis;

public sealed class ApplicationConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.ToTable("APLN", "CMN_MSTR");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("APLN_ID")
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => ApplicationId.Create(value));
        builder.Property(p => p.Name).HasColumnName("NM");
    }
}
