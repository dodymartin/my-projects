using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinimalApi.Dom.WebApis.Dtos;

namespace MinimalApi.Infra
{
    internal class ControllerUriFacilityInfoByApplicationDtoConfiguration : IEntityTypeConfiguration<ControllerUriFacilityInfoByApplicationDto>
    {
        public void Configure(EntityTypeBuilder<ControllerUriFacilityInfoByApplicationDto> builder)
        {
            builder.HasNoKey();
            builder.Property(p => p.Address);
            builder.Property(p => p.ApplicationId);
            builder.Property(p => p.ApplicationName);
            builder.Property(p => p.ApplicationVersion);
            builder.Property(p => p.EnvironmentType);
            builder.Property(p => p.FacilityId);
            builder.Property(p => p.Order);
            builder.Property(p => p.Port);
            builder.Property(p => p.UriName);
            builder.Property(p => p.UseHttps);
            builder.Property(p => p.Version);
        }
    }
}
