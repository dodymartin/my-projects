using System.CodeDom;
using System.Diagnostics.Contracts;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.ApiCallUsages;

public class ApiCallUsageDbContext : DbContext, IApiCallUsageDbContext
{
    private readonly ILogger<ApiCallUsageDbContext> _logger;
    private readonly Stratos.Core.Data.AppSettings _appSettings;
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;

    public IDbContextSettings Settings { get; }
    Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade IApiCallUsageDbContext.Database => Database;

    public DbSet<ApiCallUsage> ApiCallUsages { get; private set; } = null!;

    public ApiCallUsageDbContext(ILogger<ApiCallUsageDbContext> logger, IOptions<Stratos.Core.Data.AppSettings> appDataSettings, DbContextOptions<ApiCallUsageDbContext> options, IDbContextSettings settings, PublishDomainEventsInterceptor publishDomainEventsInterceptor)
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
                && type.GetInterfaces().Any(i=>i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
            select type;
        foreach (var type in types)
        {
            dynamic configurationInstance = Activator.CreateInstance(type);
            modelBuilder.ApplyConfiguration(configurationInstance);
        }
    }
}
