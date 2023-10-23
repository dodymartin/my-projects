using Microsoft.EntityFrameworkCore;
using MinimalApi.Dom.ApiCallUsages;
using MinimalApi.Dom.Applications;
using MinimalApi.Dom.Databases;
using MinimalApi.Dom.WebApis;

namespace MinimalApi.App;

public interface IMinimalApiDbContext
{
    Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade Database { get; }
    DbSet<ApiCallUsage> ApiCallUsages { get; }
    DbSet<Application> Applications { get; }
    DbSet<Database> Databases { get; }
    IDbContextSettings Settings { get; }
    DbSet<WebApi> WebApis { get; }
}