using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MinimalApi.Api.Features.WebApis;

public class WebApiVersionDtoConfiguration : IEntityTypeConfiguration<WebApiVersionDto>
{
    public void Configure(EntityTypeBuilder<WebApiVersionDto> builder)
    {
        builder.HasNoKey();
        builder.Property(p => p.ApplicationId);
        builder.Property(p => p.Port);
        builder.Property(p => p.UseHttps);
        builder.Property(p => p.Version);
        builder.Property(p => p.WebApiId);
    }
}
