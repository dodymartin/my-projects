using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MinimalApi.Dal;

public class MinimalApiDbContext : DbContext
{
    private readonly ILogger<MinimalApiDbContext> _logger;
    private readonly Stratos.Core.Data.AppSettings _appDataSettings;

    public DbContextSettings Settings { get; }

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

    public MinimalApiDbContext(ILogger<MinimalApiDbContext> logger, IOptions<Stratos.Core.Data.AppSettings> appDataSettings, DbContextSettings settings)
    {
        _logger = logger;
        _appDataSettings = appDataSettings.Value;
        Settings = settings;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _logger.LogDebug("Configuring for {databaseName}", Settings.DatabaseName);
        optionsBuilder.UseOracle(Settings.Database.ConnectString);
        if (_appDataSettings.ShowSql)
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(b => b.AddConsole()));
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _logger.LogDebug("Configuring Model for {databaseName}", Settings.DatabaseName);
        //modelBuilder.Entity<ApiCallUsage>(e =>
        //{
        //    e.ToTable("SVC_CALL_USG", "CMN");
        //    e.Property(p => p.Id).HasColumnName("SVC_CALL_USG_GUID");
        //    e.Property(p => p.BasicUsername).HasColumnName("BSIC_USR_NM");
        //    e.Property(p => p.Body).HasColumnName("RQST_BLOB").HasColumnType("BLOB");
        //    e.Property(p => p.CreateOrigin).HasColumnName("CRT_ORGN");
        //    //e.Property(p => p.ElapsedMilliSeconds).HasColumnName("ELPSD_MILLI_SECS");
        //    e.Property(p => p.HasAuthorizationHeader).HasColumnName("HAS_AUTH_HDR");
        //    e.Property(p => p.ApiApplicationExeName).HasColumnName("SVC_NM");
        //    e.Property(p => p.ApiApplicationVersion).HasColumnName("SVC_ASMBLY_VER");
        //    //e.Property(p => p.LocalIpAddress).HasColumnName("LCL_IP_ADDR");
        //    e.Property(p => p.ApiMachineName).HasColumnName("SVC_MACH_NM");
        //    //e.Property(p => p.LocalProcessId).HasColumnName("LCL_PRCS_ID");
        //    e.Property(p => p.MethodName).HasColumnName("MTHD_NM");
        //    e.Property(p => p.RequestApplicationExeName).HasColumnName("EXE_NM");
        //    e.Property(p => p.RequestApplicationVersion).HasColumnName("EXE_VER");
        //    e.Property(p => p.RequestIpAddress).HasColumnName("IP_ADDR");
        //    e.Property(p => p.RequestMachineName).HasColumnName("MACH_NM");
        //    e.Property(p => p.RequestProcessId).HasColumnName("PRCS_ID");
        //    //e.Property(p => p.Url).HasColumnName("URL");
        //});
        //modelBuilder.Entity<Application>(e =>
        //{
        //    e.ToTable("APLN", "CMN_MSTR");
        //    e.Property(p => p.Id).HasColumnName("APLN_ID");
        //    e.Property(p => p.ExeName).HasColumnName("EXE_NM");
        //    e.Property(p => p.FromDirectoryName).HasColumnName("FROM_DIR_NM");
        //    e.Property(p => p.Name).HasColumnName("NM");
        //    e.Property(p => p.MinimumAssemblyVersion).HasColumnName("MIN_ASMBLY_VER");

        //    e.HasMany(e => e.Versions).WithOne();
        //});
        //modelBuilder.Entity<ApplicationFacility>(e =>
        //{
        //    e.ToTable("APLN_FAC", "CMN_MSTR");
        //    e.Property(p => p.Id).HasColumnName("APLN_FAC_ID");
        //    e.Property(p => p.ApplicationId).HasColumnName("APLN_ID");
        //    e.Property(p => p.FacilityId).HasColumnName("FAC_ID");
        //    e.Property(p => p.MinimumAssemblyVersion).HasColumnName("MIN_ASMBLY_VER");
        //});
        //modelBuilder.Entity<ApplicationVersion>(e =>
        //{
        //    e.ToTable("APLN_VER", "CMN_MSTR");
        //    e.Property(p => p.Id).HasColumnName("APLN_VER_ID");
        //    e.Property(p => p.ApplicationId).HasColumnName("APLN_ID");
        //    e.Property(p => p.FromDirectoryName).HasColumnName("FROM_DIR_NM");
        //    e.Property(p => p.Version).HasColumnName("VER");
        //});
        //modelBuilder.Entity<ControllerUriFacilityInfo>(e =>
        //{
        //    e.Property(p => p.Id).HasColumnName("ID");
        //    e.Property(p => p.Address).HasColumnName("ADDR");
        //    e.Property(p => p.EnvironmentType).HasColumnName("ENVIR_TP_ID");
        //    e.Property(p => p.FacilityId).HasColumnName("FAC_ID");
        //    e.Property(p => p.Order).HasColumnName("ORD");
        //    e.Property(p => p.Port).HasColumnName("PORT");
        //    e.Property(p => p.RedirectVersion).HasColumnName("RDRCT_VER");
        //    e.Property(p => p.UriName).HasColumnName("URI_NM");
        //    e.Property(p => p.UseHttps).HasColumnName("USE_HTTPS");
        //    e.Property(p => p.UseProxy).HasColumnName("USE_PROXY");
        //    e.Property(p => p.Version).HasColumnName("VER");
        //    e.Property(p => p.WebApiVersionId).HasColumnName("WEB_API_VER_ID");
        //});
        //modelBuilder.Entity<ControllerUriFacilityInfoByApplication>(e =>
        //{
        //    e.Property(p => p.Id).HasColumnName("ID");
        //    e.Property(p => p.Address).HasColumnName("ADDR");
        //    e.Property(p => p.ApplicationId).HasColumnName("APLN_ID");
        //    e.Property(p => p.ApplicationName).HasColumnName("APLN_NM");
        //    e.Property(p => p.ApplicationVersion).HasColumnName("APLN_VER");
        //    e.Property(p => p.EnvironmentType).HasColumnName("ENVIR_TP_ID");
        //    e.Property(p => p.FacilityId).HasColumnName("FAC_ID");
        //    e.Property(p => p.Order).HasColumnName("ORD");
        //    e.Property(p => p.Port).HasColumnName("PORT");
        //    e.Property(p => p.UriName).HasColumnName("URI_NM");
        //    e.Property(p => p.UseHttps).HasColumnName("USE_HTTPS");
        //    e.Property(p => p.Version).HasColumnName("VER");
        //});
        //modelBuilder.Entity<ControllerUriInfo>(e =>
        //{
        //    e.Property(p => p.Id).HasColumnName("ID");
        //    e.Property(p => p.Address).HasColumnName("ADDR");
        //    e.Property(p => p.EnvironmentType).HasColumnName("ENVIR_TP_ID");
        //    e.Property(p => p.Order).HasColumnName("ORD");
        //    e.Property(p => p.Port).HasColumnName("PORT");
        //    e.Property(p => p.RedirectVersion).HasColumnName("RDRCT_VER");
        //    e.Property(p => p.UriName).HasColumnName("URI_NM");
        //    e.Property(p => p.UseHttps).HasColumnName("USE_HTTPS");
        //    e.Property(p => p.UseProxy).HasColumnName("USE_PROXY");
        //    e.Property(p => p.Version).HasColumnName("VER");
        //    e.Property(p => p.WebApiVersionId).HasColumnName("WEB_API_VER_ID");
        //});
        //modelBuilder.Entity<ControllerUriInfoByApplication>(e =>
        //{
        //    e.Property(p => p.Id).HasColumnName("ID");
        //    e.Property(p => p.Address).HasColumnName("ADDR");
        //    e.Property(p => p.ApplicationId).HasColumnName("APLN_ID");
        //    e.Property(p => p.ApplicationName).HasColumnName("APLN_NM");
        //    e.Property(p => p.ApplicationVersion).HasColumnName("APLN_VER");
        //    e.Property(p => p.EnvironmentType).HasColumnName("ENVIR_TP_ID");
        //    e.Property(p => p.MachineName).HasColumnName("MACH_NM");
        //    e.Property(p => p.Order).HasColumnName("ORD");
        //    e.Property(p => p.Port).HasColumnName("PORT");
        //    e.Property(p => p.UriName).HasColumnName("URI_NM");
        //    e.Property(p => p.UseHttps).HasColumnName("USE_HTTPS");
        //    e.Property(p => p.Version).HasColumnName("VER");
        //});
        //modelBuilder.Entity<Database>(e =>
        //{
        //    e.ToTable("DB", "CMN_MSTR");
        //    e.Property(p => p.Id).HasColumnName("DB_ID");
        //    e.Property(p => p.EnvironmentType).HasColumnName("ENVIR_TP_ID");
        //    e.Property(p => p.SchemaType).HasColumnName("DB_SCHEMA_TP_ID");
        //    e.Property(p => p.Type).HasColumnName("DB_TP_ID");
        //    e.Property(p => p.Name).HasColumnName("NM");
        //    e.Property(p => p.ParentId).HasColumnName("PRNT_DB_ID");

        //    e.HasMany(e => e.Facilities).WithMany(e => e.Databases).UsingEntity<DatabaseFacility>();
        //});
        //modelBuilder.Entity<DatabaseFacility>(e =>
        //{
        //    e.ToTable("DB_FAC", "CMN_MSTR");
        //    e.Property(p => p.DatabaseId).HasColumnName("DB_ID");
        //    e.Property(p => p.FacilityId).HasColumnName("FAC_ID");
        //});
        //modelBuilder.Entity<Facility>(e =>
        //{
        //    e.ToTable("FAC", "CMN_MSTR");
        //    e.Property(p => p.Id).HasColumnName("FAC_ID");
        //    e.Property(p => p.Name).HasColumnName("NM");

        //    e.HasMany(e => e.Databases).WithMany(e => e.Facilities).UsingEntity<DatabaseFacility>();
        //});
        //modelBuilder.Entity<WebApi>(e =>
        //{
        //    e.ToTable("WEB_API", "CMN_MSTR");
        //    e.Property(p => p.Id).HasColumnName("WEB_API_ID");
        //    e.Property(p => p.ApplicationId).HasColumnName("APLN_ID");
        //    e.Property(p => p.UseHttps).HasColumnName("USE_HTTPS");
        //});
        //modelBuilder.Entity<WebApiController>(e =>
        //{
        //    e.ToTable("WEB_API_CTLR", "CMN_MSTR");
        //    e.Property(p => p.Id).HasColumnName("WEB_API_CTLR_ID");
        //    e.Property(p => p.UriName).HasColumnName("URI_NM");
        //    e.Property(p => p.WebApiId).HasColumnName("WEB_API_ID");
        //});
        //modelBuilder.Entity<WebApiVersion>(e =>
        //{
        //    e.ToTable("WEB_API_VER", "CMN_MSTR");
        //    e.Property(p => p.Id).HasColumnName("WEB_API_VER_ID");
        //    e.Property(p => p.Port).HasColumnName("PORT");
        //    e.Property(p => p.Version).HasColumnName("VER");
        //    e.Property(p => p.WebApiId).HasColumnName("WEB_API_ID");
        //});
    }
}
