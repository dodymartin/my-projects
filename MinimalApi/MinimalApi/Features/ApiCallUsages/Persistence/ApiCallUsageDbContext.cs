using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.ApiCallUsages;

public class ApiCallUsageDbContext(ILogger<ApiCallUsageDbContext> logger, IOptions<AppSettings> appSettings, DbContextOptions<ApiCallUsageDbContext> options, IDbContextSettings settings, SaveChangesInterceptor saveChangesInterceptor)
    : DbContext(options), IApiCallUsageDbContext
{
    private readonly ILogger<ApiCallUsageDbContext> _logger = logger;
    private readonly AppSettings _appSettings = appSettings.Value;
    private readonly SaveChangesInterceptor _saveChangesInterceptor = saveChangesInterceptor;

    public IDbContextSettings Settings { get; } = settings;
    Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade IApiCallUsageDbContext.Database => Database;

    public DbSet<ApiCallUsage> ApiCallUsages { get; private set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _logger.LogDebug("Configuring for {databaseName}", Settings.DatabaseName);

        var connectString = "Data Source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP) (Host = xeg6client03-vip.admin.cargill.com) (Port = 1521)) (CONNECT_DATA = (SID = qpfsnspt)));User Id=ps900183;Password=Q1w2e3r4;";
        optionsBuilder.UseOracle(connectString);
        //optionsBuilder.UseOracle(Settings.Database.ConnectString);
        if (_appSettings.ShowSql)
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(b => b.AddConsole()));
        optionsBuilder.AddInterceptors(_saveChangesInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _logger.LogDebug("Creating Model for {databaseName}", Settings.DatabaseName);
        modelBuilder
            .Ignore<List<IDomainEvent>>();

        var types =
            from type in Assembly.GetExecutingAssembly().GetTypes()
            where type.Namespace == GetType().Namespace
                && !type.IsAbstract
                && !type.IsInterface
                && type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
            select type;
        foreach (var type in types)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            dynamic configurationInstance = Activator.CreateInstance(type);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            modelBuilder.ApplyConfiguration(configurationInstance);
        }
    }
}
