using Microsoft.EntityFrameworkCore;
using MinimalApi.Dom.ApiCallUsages;
using MinimalApi.Dom.Applications;
using MinimalApi.Dom.Databases;
using MinimalApi.Dom.WebApis;
using MinimalApi.Dom.WebApis.Dtos;

namespace MinimalApi.App;

public interface IMinimalApiDbContext
{
    Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade Database { get; }
    DbSet<ApiCallUsage> ApiCallUsages { get; }
    DbSet<Application> Applications { get; }
    DbSet<ControllerUriFacilityInfoByApplicationDto> ControllerUriFacilityInfoByApplicationDtos { get; }
    DbSet<ControllerUriInfoByApplicationDto> ControllerUriInfoByApplicationDtos { get; }
    DbSet<Database> Databases { get; }
    IDbContextSettings Settings { get; }
    DbSet<WebApi> WebApis { get; }
    DbSet<WebApiVersionDto> WebApiVersionDtos { get; }
}