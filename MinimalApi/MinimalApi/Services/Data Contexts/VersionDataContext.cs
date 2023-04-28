using Microsoft.EntityFrameworkCore;
using MinimalApi.Entities;
using Stratos.Core;

namespace MinimalApi.Version;

public class VersionDataContext : DbContext, IVersionDataContext
{
    private readonly ILogger<VersionDataContext> _logger;
    private readonly VersionDataContextSettings _contextSettings;
    private readonly IDatabases _dbs;

    public DbSet<Application> Applications { get; private set; }
    public DbSet<ApplicationFacility> ApplicationFacilities { get; private set; }

    public VersionDataContext(ILogger<VersionDataContext> logger, VersionDataContextSettings contextSettings, IDatabases dbs)
    {
        _logger = logger;
        _contextSettings = contextSettings;
        _dbs = dbs;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _logger.LogDebug("Configuring for {databaseName}", _contextSettings.DatabaseName);
        optionsBuilder.UseOracle(_dbs[_contextSettings.DatabaseName].ConnectString);
        optionsBuilder.UseLoggerFactory(LoggerFactory.Create(b => b.AddConsole()));
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _logger.LogDebug("Configuring Model for {databaseName}", _contextSettings.DatabaseName);
        modelBuilder.Entity<Application>(e =>
        {
            e.ToTable("APLN", "CMN_MSTR");
            e.Property(p => p.Id).HasColumnName("APLN_ID");
            e.Property(p => p.ExeName).HasColumnName("EXE_NM");
            e.Property(p => p.FromDirectoryName).HasColumnName("FROM_DIR_NM");
            e.Property(p => p.Name).HasColumnName("NM");
            e.Property(p => p.MinimumAssemblyVersion).HasColumnName("MIN_ASMBLY_VER");
        });
        modelBuilder.Entity<ApplicationFacility>(e =>
        {
            e.ToTable("APLN_FAC", "CMN_MSTR");
            e.Property(p => p.Id).HasColumnName("APLN_FAC_ID");
            e.Property(p => p.ApplicationId).HasColumnName("APLN_ID");
            e.Property(p => p.FacilityId).HasColumnName("FAC_ID");
            e.Property(p => p.MinimumAssemblyVersion).HasColumnName("MIN_ASMBLY_VER");
        });
        modelBuilder.Entity<Facility>(e =>
        {
            e.ToTable("FAC", "CMN_MSTR");
            e.Property(p => p.Id).HasColumnName("FAC_ID");
            e.Property(p => p.Name).HasColumnName("NM");
        });
    }
}
