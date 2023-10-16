using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MinimalApi.App;
using MinimalApi.Dom.ApiCallUsages;
using MinimalApi.Dom.Applications.Entities;
using MinimalApi.Dom.Databases;
using MinimalApi.Dom.WebApis;
using MinimalApi.Dom.WebApis.Entities;
using Application = MinimalApi.Dom.Applications.Application;

namespace MinimalApi.Infra.Persistence;

public class MinimalApiDbContext : DbContext, IMinimalApiDbContext
{
    private readonly ILogger<MinimalApiDbContext> _logger;
    private readonly Stratos.Core.Data.AppSettings _appSettings;

    public IDbContextSettings Settings { get; }

    public DbSet<ApiCallUsage> ApiCallUsages { get; private set; } = null!;
    public DbSet<Application> Applications { get; private set; } = null!;
    public DbSet<ApplicationFacility> ApplicationFacilities { get; private set; } = null!;
    public DbSet<ControllerUriFacilityInfo> ControllerUriFacilityInfos { get; private set; } = null!;
    public DbSet<ControllerUriFacilityInfoByApplication> ControllerUriFacilityInfosByApplication { get; private set; } = null!;
    public DbSet<ControllerUriInfo> ControllerUriInfos { get; private set; } = null!;
    public DbSet<ControllerUriInfoByApplication> ControllerUriInfosByApplication { get; private set; } = null!;
    public DbSet<Database> Databases { get; private set; } = null!;
    public DbSet<WebApi> WebApis { get; private set; } = null!;
    public DbSet<WebApiController> WebApiControllers { get; private set; } = null!;
    public DbSet<WebApiVersion> WebApiVersions { get; private set; } = null!;

    public MinimalApiDbContext(ILogger<MinimalApiDbContext> logger, IOptions<Stratos.Core.Data.AppSettings> appDataSettings, DbContextOptions<MinimalApiDbContext> options, IDbContextSettings settings)
        :base(options)
    {
        _logger = logger;
        _appSettings = appDataSettings.Value;
        Settings = settings;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _logger.LogDebug("Configuring for {databaseName}", Settings.DatabaseName);
        optionsBuilder.UseOracle(Settings.Database.ConnectString);
        if (_appSettings.ShowSql)
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(b => b.AddConsole()));
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _logger.LogDebug("Creating Model for {databaseName}", Settings.DatabaseName);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
