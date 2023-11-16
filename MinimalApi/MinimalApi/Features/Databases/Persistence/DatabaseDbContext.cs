using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.Databases;

public sealed class DatabaseDbContext(ILogger<DatabaseDbContext> logger, IOptions<AppSettings> appSettings, DbContextOptions<DatabaseDbContext> options, IDbContextSettings settings, PublishDomainEventsInterceptor publishDomainEventsInterceptor) 
    : DbContext(options), IDatabaseDbContext
{
    private readonly ILogger<DatabaseDbContext> _logger = logger;
    private readonly AppSettings _appSettings = appSettings.Value;
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor = publishDomainEventsInterceptor;

    public IDbContextSettings Settings { get; } = settings;
    Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade IDatabaseDbContext.Database => Database;

    public DbSet<Database> Databases { get; private set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _logger.LogDebug("Configuring for {databaseName}", Settings.DatabaseName);

        var connectString = "Data Source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP) (Host = xeg6client03-vip.admin.cargill.com) (Port = 1521)) (CONNECT_DATA = (SID = qpfsnspt)));User Id=ps900183;Password=Q1w2e3r4;";
        optionsBuilder.UseOracle(connectString);
        //optionsBuilder.UseOracle(Settings.Database.ConnectString);
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
