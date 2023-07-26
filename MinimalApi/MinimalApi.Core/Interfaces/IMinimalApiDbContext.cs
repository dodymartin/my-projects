using Microsoft.EntityFrameworkCore;

namespace MinimalApi.Core
{
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
}