using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MinimalApi.App;
using MinimalApi.Dom.ApiCallUsages;
using MinimalApi.Dom.Common.Models;
using MinimalApi.Dom.Databases;
using MinimalApi.Dom.WebApis;
using MinimalApi.Dom.WebApis.Dtos;
using MinimalApi.Infra.Persistence.Interceptors;
using Application = MinimalApi.Dom.Applications.Application;

namespace MinimalApi.Infra.Persistence;

public class MinimalApiDbContext : DbContext, IMinimalApiDbContext
{
    private readonly ILogger<MinimalApiDbContext> _logger;
    private readonly Stratos.Core.Data.AppSettings _appSettings;
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;

    public IDbContextSettings Settings { get; }

    public DbSet<ApiCallUsage> ApiCallUsages { get; private set; } = null!;
    public DbSet<Application> Applications { get; private set; } = null!;
    public DbSet<ControllerUriFacilityInfoByApplicationDto> ControllerUriFacilityInfoByApplicationDtos { get; private set; } = null!;
    public DbSet<ControllerUriInfoByApplicationDto> ControllerUriInfoByApplicationDtos { get; private set; } = null!;
    public DbSet<Database> Databases { get; private set; } = null!;
    public DbSet<WebApi> WebApis { get; private set; } = null!;
    public DbSet<WebApiVersionDto> WebApiVersionDtos { get; private set; } = null!;

    Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade IMinimalApiDbContext.Database => Database;

    public MinimalApiDbContext(ILogger<MinimalApiDbContext> logger, IOptions<Stratos.Core.Data.AppSettings> appDataSettings, DbContextOptions<MinimalApiDbContext> options, IDbContextSettings settings, PublishDomainEventsInterceptor publishDomainEventsInterceptor)
        :base(options)
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
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
