using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MinimalApi.Api.Features.WebApis;

internal class ControllerUriInfoByApplicationDtoConfiguration : IEntityTypeConfiguration<ControllerUriInfoByApplicationDto>
{
    public void Configure(EntityTypeBuilder<ControllerUriInfoByApplicationDto> builder)
    {
        builder.HasNoKey();
        builder.Property(p => p.Address);
        builder.Property(p => p.ApplicationId);
        builder.Property(p => p.ApplicationName);
        builder.Property(p => p.ApplicationVersion);
        builder.Property(p => p.EnvironmentType);
        builder.Property(p => p.MachineName);
        builder.Property(p => p.Order);
        builder.Property(p => p.Port);
        builder.Property(p => p.UriName);
        builder.Property(p => p.UseHttps);
        builder.Property(p => p.Version);
    }
}
