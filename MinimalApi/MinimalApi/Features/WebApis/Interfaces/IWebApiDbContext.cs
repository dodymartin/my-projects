using Microsoft.EntityFrameworkCore;
using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.WebApis;

public interface IWebApiDbContext
{
    Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade Database { get; }
    IDbContextSettings Settings { get; }

    DbSet<Application> Applications { get; }
    DbSet<ControllerUriFacilityInfoByApplicationDto> ControllerUriFacilityInfoByApplicationDtos { get; }
    DbSet<ControllerUriInfoByApplicationDto> ControllerUriInfoByApplicationDtos { get; }
    DbSet<WebApi> WebApis { get; }
    DbSet<WebApiVersionDto> WebApiVersionDtos { get; }
}