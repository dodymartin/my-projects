using Microsoft.EntityFrameworkCore;
using MinimalApi.Dom.ApiCallUsages;
using MinimalApi.Dom.Applications;
using MinimalApi.Dom.Applications.Entities;
using MinimalApi.Dom.Databases;
using MinimalApi.Dom.WebApis;
using MinimalApi.Dom.WebApis.Entities;

namespace MinimalApi.App;

public interface IMinimalApiDbContext
{
    DbSet<ApiCallUsage> ApiCallUsages { get; }
    DbSet<ApplicationFacility> ApplicationFacilities { get; }
    DbSet<Application> Applications { get; }
    DbSet<ControllerUriFacilityInfo> ControllerUriFacilityInfos { get; }
    DbSet<ControllerUriFacilityInfoByApplication> ControllerUriFacilityInfosByApplication { get; }
    DbSet<ControllerUriInfo> ControllerUriInfos { get; }
    DbSet<ControllerUriInfoByApplication> ControllerUriInfosByApplication { get; }
    DbSet<Database> Databases { get; }
    IDbContextSettings Settings { get; }
    DbSet<WebApiController> WebApiControllers { get; }
    DbSet<WebApi> WebApis { get; }
    DbSet<WebApiVersion> WebApiVersions { get; }
}