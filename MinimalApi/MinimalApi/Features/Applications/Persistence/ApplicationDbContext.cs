using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.Applications;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly ILogger<ApplicationDbContext> _logger;
    private readonly AppSettings _appSettings;
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;

    public IDbContextSettings Settings { get; }
    Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade IApplicationDbContext.Database => Database;

    public DbSet<Application> Applications { get; private set; } = null!;

    public ApplicationDbContext(ILogger<ApplicationDbContext> logger, IOptions<AppSettings> appSettings, DbContextOptions<ApplicationDbContext> options, IDbContextSettings settings, PublishDomainEventsInterceptor publishDomainEventsInterceptor)
        : base(options)
    {
        _logger = logger;
        _appSettings = appSettings.Value;
        Settings = settings;
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

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
