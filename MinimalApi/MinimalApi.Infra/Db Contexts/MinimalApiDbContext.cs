using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MinimalApi.Core;

namespace MinimalApi.Infra;

public class MinimalApiDbContext : DbContext, IMinimalApiDbContext
{
    private readonly ILogger<MinimalApiDbContext> _logger;
    private readonly Stratos.Core.Data.AppSettings _appSettings;

    public IDbContextSettings Settings { get; }

    public DbSet<ApiCallUsage> ApiCallUsages { get; private set; }
    public DbSet<Application> Applications { get; private set; }
    public DbSet<ApplicationFacility> ApplicationFacilities { get; private set; }
    public DbSet<ControllerUriFacilityInfo> ControllerUriFacilityInfos { get; private set; }
    public DbSet<ControllerUriFacilityInfoByApplication> ControllerUriFacilityInfosByApplication { get; private set; }
    public DbSet<ControllerUriInfo> ControllerUriInfos { get; private set; }
    public DbSet<ControllerUriInfoByApplication> ControllerUriInfosByApplication { get; private set; }
    public DbSet<Database> Databases { get; private set; }
    public DbSet<WebApi> WebApis { get; private set; }
    public DbSet<WebApiController> WebApiControllers { get; private set; }
    public DbSet<WebApiVersion> WebApiVersions { get; private set; }

    public MinimalApiDbContext(ILogger<MinimalApiDbContext> logger, IOptions<Stratos.Core.Data.AppSettings> appDataSettings, IDbContextSettings settings)
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
