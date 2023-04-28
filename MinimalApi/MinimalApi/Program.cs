using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MinimalApi;
using MinimalApi.Version;
using NLog.Extensions.Logging;
using Stratos.Core;

if (Environment.UserInteractive)
{
    Console.WriteLine("Application is paused. If needed, attach");
    Console.WriteLine("remote debugger now.");
    Console.WriteLine("\nPress any key to continue!");
    Console.ReadKey();
}

try
{
    #region Get General/NLog Configuration

    Environment.CurrentDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName)!;

    // Load IConfiguration from .json files
    var config = Stratos.Core.CoreMethods.BuildConfiguration(Environment.CurrentDirectory)
        .Build();

    // Setup logger directly for startup
    NLog.LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));
    NLog.LogManager.GetCurrentClassLogger().Log(NLog.LogLevel.Info, "Starting");

    #endregion

    //if (Environment.UserInteractive && !Stratos.Core.CoreMethods.IsAdministrator())
    //    throw new Exception("This app must run in as an administrator!");

    //var jsonTypeInfoResolver = new JsonTypeInfoResolver();
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddConfiguration(config);

    builder.Services.AddLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddNLog(config.GetSection("NLog"));
    });

    builder.Services.Configure<ConsoleLifetimeOptions>(opt => opt.SuppressStatusMessages = true);

    builder.Services.Configure<JsonOptions>(opt =>
    {
        opt.SerializerOptions.PropertyNameCaseInsensitive = true;
        opt.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        opt.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opt.SerializerOptions.WriteIndented = true;
        //opt.SerializerOptions.TypeInfoResolver = jsonTypeInfoResolver;
    });

    builder.Services.AddRouting();

    //builder.Services.AddAuthentication(opt =>
    //{
    //    opt.DefaultAuthenticateScheme = BasicDefaults.AuthenticationScheme;
    //    opt.DefaultChallengeScheme = BasicDefaults.AuthenticationScheme;
    //})
    //.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicDefaults.AuthenticationScheme, null)
    //.AddJwtBearer(opts =>
    //{
    //    opts.SaveToken = true;
    //    opts.TokenValidationParameters = JwtConfiguration.GetTokenValidationParameters();
    //});

    builder.Services.AddServices(builder.Configuration);

    var app = builder.Build();

    app.UseHttpsRedirection();
    app.UseRouting();

    //app.UseAuthentication();
    //app.UseAuthorization();

    app.UseVersionWebApi();

    app.Run();

    //await DoProcessingAsync(args, config);
}
catch (Exception ex)
{
    NLog.LogManager.GetCurrentClassLogger().Log(NLog.LogLevel.Error, ex.ToString());
    Environment.ExitCode = 10001;
}

NLog.LogManager.GetCurrentClassLogger().Log(NLog.LogLevel.Info, "Stopping");

if (Environment.UserInteractive)
    Console.ReadKey();

//static async Task DoProcessingAsync(string[] args, IConfiguration config)
//{
//    //var baseUri = Initialize(config);

//    var customContractResolver = new CustomContractResolver();
//    var host = Host.CreateDefaultBuilder(args)
//        .UseWindowsService()
//        .ConfigureAppConfiguration(builder =>
//        {
//            // Commented out, since it will clear the Url configured below.
//            //builder.Sources.Clear();
//            builder.AddConfiguration(config);
//        })
//        .ConfigureWebHostDefaults(webBuilder =>
//        {
//            #region Configure for Http

//            webBuilder.UseContentRoot(Environment.CurrentDirectory);
//            //webBuilder.UseUrls(new string[] { baseUri });

//            //// The following UseHttpSys gets the service, if listening on https,
//            //// to use the certificate bound to the port that was added using
//            //// netsh, otherwise, you would have to load the server certificate
//            //// in ConfigureServices.This ties the service to only hosting on
//            //// Windows, which is not currently a problem.
//            //webBuilder.UseHttpSys();
//            webBuilder.Configure(app =>
//            {
//                //app.UseOwin();
//                app.UseRouting();

//                app.UseAuthentication();
//                app.UseAuthorization();

//                app.UseEndpoints(endpoints => endpoints.MapControllers());
//            });

//            #endregion
//        })
//        .ConfigureServices((_, services) =>
//        {
//            #region Configure for Web Api

//            services.AddControllers()
//                .AddNewtonsoftJson(opt =>
//                {
//                    opt.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
//                    opt.SerializerSettings.Formatting = Formatting.Indented;
//                    opt.SerializerSettings.ContractResolver = customContractResolver;
//                })
//                .AddApplicationPart(Assembly.GetEntryAssembly())
//                .AddControllersAsServices();

//            services.AddRouting();

//            services.AddAuthentication(opt =>
//            {
//                opt.DefaultAuthenticateScheme = BasicDefaults.AuthenticationScheme;
//                opt.DefaultChallengeScheme = BasicDefaults.AuthenticationScheme;
//            })
//            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicDefaults.AuthenticationScheme, null)
//            .AddJwtBearer(opts =>
//            {
//                opts.SaveToken = true;
//                opts.TokenValidationParameters = JwtConfiguration.GetTokenValidationParameters();
//            });

//            services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);

//            #endregion

//            services.AddHttpContextAccessor();
//            services.AddServices(config);
//        })
//        .Build();

//    customContractResolver.Initialize(host.Services);
//    await host.RunAsync();
//}

//static string Initialize(IConfiguration config)
//{
//    var services = new ServiceCollection();
//    services.AddServices(config);
//    using var serviceProvider = services.BuildServiceProvider();
//    var logger = serviceProvider.GetRequiredService<ILogger<object>>();

//    #region Checking Environment Type

//    var appSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value;
//    if (!string.IsNullOrEmpty(appSettings.DatabaseName))
//    {
//        var databaseName = appSettings.DatabaseName;
//        logger.LogInformation("Checking that Environment Type matches for {databaseName}", appSettings.DatabaseName);

//        // Get current database's environment type
//        var queryEngineFactory = serviceProvider.GetRequiredService<IQueryEngineFactory>();
//        using var qe = queryEngineFactory.Create(databaseName);
//        var db =
//            (from d in qe.Query<Database>()
//             where d.Name == databaseName
//             select d)
//            .FirstOrDefault();
//        if (db != null)
//        {
//            var environmentType = (EnvironmentTypes)Enum.Parse(typeof(EnvironmentTypes), appSettings.EnvironmentType);
//            if (db.EnvironmentType.Id != environmentType)
//                throw new Exception($"The Environment Type for {databaseName} ({db.EnvironmentType.Id}) does not match value found in appsettings.json ({environmentType}), so service will not start!");

//            // Refresh the cache files at startup
//            var cacheService = serviceProvider.GetRequiredService<ICacheService>();
//            cacheService.Refresh(databaseName, environmentType);
//        }
//    }

//    #endregion

//    logger.LogInformation("Listening on: {baseUri}", appSettings.BaseUri);

//    return appSettings.BaseUri;
//}

public static class Services
{
    public static void AddServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddBootstrapServices();

        // Register IApplicationContext to force feeding ApplicationId
        services.TryAddSingleton<IApplicationContext>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<ApplicationContext>>();
            return new ApplicationContext(
                logger,
                (int)Applications.StratosConfigurationWebApiHost);
        });

        // Register IServiceSideUsers (serviceSideUsers.json)
        services.TryAddSingleton(ServiceSideUsersLoader.Load());

        // Register each typed AppSettings class to Section in appsettings.json
        services.Configure<MinimalApi.AppSettings>(config.GetSection("AppSettings"));
        services.Configure<Stratos.Core.AppSettings>(config.GetSection("Stratos.Core.AppSettings"));
        services.Configure<Stratos.Core.Data.AppSettings>(config.GetSection("Stratos.Core.Data.AppSettings"));

        //// Register ISessionBuilder, IQueryEngine and IQueryEngine
        //services.TryAddSingleton<ISessionBuilder, SessionBuilder>();
        //services.AddQueryEngineFactory(ServiceLifetime.Transient);
        //services.AddStatelessQueryEngineFactory(ServiceLifetime.Transient);

        // Register Data services
        //services.TryAddTransient<IAuthenticationDataService, AuthenticationDataService>();
        //services.TryAddTransient<IBaseUriParmsDataService, BaseUriParmsDataService>();
        //services.TryAddTransient<ICacheDataService, CacheDataService>();
        //services.TryAddTransient<IControllerUriParmsDataService, ControllerUriParmsDataService>();
        //services.TryAddTransient<IDatabaseNameDataService, DatabaseNameDataService>();
        //services.TryAddTransient<IPingUriParmsDataService, PingUriParmsDataService>();
        //services.TryAddTransient<IUriDataService, UriDataService>();
        //services.TryAddTransient<IUsageDataService, UsageDataService>();
        services.TryAddScoped<VersionDataContextSettings>();
        services.TryAddScoped<IVersionDataContext, VersionDataContext>();

        // Register Worker services
        //services.TryAddSingleton<Stratos.Core.WebApi.IAuthenticationService, Stratos.Core.WebApi.AuthenticationService>();
        //services.TryAddSingleton<ICacheService, CacheService>();
        //services.TryAddScoped<IDatabaseNameService, DatabaseNameService>();
        //services.TryAddScoped<IUriService, UriService>();
        services.TryAddScoped<IVersionService, VersionService>();
    }
}