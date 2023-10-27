using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.WebApis;

public class WebApiDbContext : DbContext, IWebApiDbContext
{
    private readonly ILogger<WebApiDbContext> _logger;
    private readonly Stratos.Core.Data.AppSettings _appSettings;
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;

    public IDbContextSettings Settings { get; }
    Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade IWebApiDbContext.Database => Database;

    public DbSet<Application> Applications { get; private set; } = null!;
    public DbSet<ControllerUriFacilityInfoByApplicationDto> ControllerUriFacilityInfoByApplicationDtos { get; private set; } = null!;
    public DbSet<ControllerUriInfoByApplicationDto> ControllerUriInfoByApplicationDtos { get; private set; } = null!;
    public DbSet<WebApi> WebApis { get; private set; } = null!;
    public DbSet<WebApiVersionDto> WebApiVersionDtos { get; private set; } = null!;

    public WebApiDbContext(ILogger<WebApiDbContext> logger, IOptions<Stratos.Core.Data.AppSettings> appDataSettings, DbContextOptions<WebApiDbContext> options, IDbContextSettings settings, PublishDomainEventsInterceptor publishDomainEventsInterceptor)
        : base(options)
    {
        _logger = logger;
        _appSettings = appDataSettings.Value;
        Settings = settings;
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _logger.LogDebug("Configuring for {databaseName}", Settings.DatabaseName);
        optionsBuilder.UseOracle(Settings.Database.ConnectString);
        if (_appSettings.ShowSql)
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(b => b.AddConsole()));
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _logger.LogDebug("Creating Model for {databaseName}", Settings.DatabaseName);
        modelBuilder
            .Ignore<List<IDomainEvent>>();

        var types = from type in Assembly.GetExecutingAssembly().GetTypes()
                    where type.Namespace == GetType().Namespace
                        && !type.IsAbstract
                        && !type.IsInterface
                        && type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                    select type;
        foreach (var type in types)
        {
            dynamic configurationInstance = Activator.CreateInstance(type);
            modelBuilder.ApplyConfiguration(configurationInstance);
        }
    }
}
