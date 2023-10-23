using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Carter;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MinimalApi.Api;
using MinimalApi.App;
using MinimalApi.Dom.Enumerations;
using MinimalApi.Infra;
using NLog.Extensions.Logging;
using Stratos.Core;
using Stratos.Core.WebApi;

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

    var builder = WebApplication.CreateBuilder(args);

    #region Setup Configuration, Logging, and Routing

    builder.Configuration.AddConfiguration(config);

    builder.Services.AddLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddNLog(config.GetSection("NLog"));
    });

    builder.Services.Configure<ConsoleLifetimeOptions>(opt => opt.SuppressStatusMessages = true);
    builder.Services.AddRouting();

    #endregion

    #region Setup JSON.NET formatting

    builder.Services.Configure<JsonOptions>(opt =>
    {
        opt.SerializerOptions.PropertyNameCaseInsensitive = true;
        opt.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        opt.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opt.SerializerOptions.WriteIndented = true;
    });

    #endregion

    #region Add Authentication/Authorization

    // Add Basic Authorization
    builder.Services.AddAuthentication()
        .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicDefaults.AuthenticationScheme, null);
    builder.Services.AddAuthorizationBuilder()
        .AddPolicy(BasicDefaults.AuthenticationScheme, policy =>
        {
            policy.RequireAuthenticatedUser()
                .AuthenticationSchemes.Add(BasicDefaults.AuthenticationScheme);
        });

    // Add JWT Authorization
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opts =>
        {
            opts.SaveToken = true;
            opts.TokenValidationParameters = JwtConfiguration.GetTokenValidationParameters();
        });
    builder.Services.AddAuthorization();

    #endregion

    #region Add Services

    // TODO: From AddBootstrapServices, create new methods
    //  AddCoreLoaderServices and AddCoreHttpServices, do not
    //  include AuthenticationWebApi and ConfigurationWebApi

    //builder.Services.AddBootstrapServices();
    builder.Services.TryAddSingleton(ClientSideUsersLoader.Load());
    builder.Services.TryAddSingleton(DatabasesLoader.Load());
    builder.Services.TryAddSingleton(SiteConfigurationLoader.Load());

    // Register IApplicationContext to force feeding ApplicationId
    builder.Services.TryAddSingleton<IApplicationContext>(sp =>
    {
        var logger = sp.GetRequiredService<ILogger<ApplicationContext>>();
        return new ApplicationContext(
            logger,
            (int)Applications.StratosConfigurationWebApiHost);
    });

    // Register IServiceSideUsers (serviceSideUsers.json)
    builder.Services.TryAddSingleton(ServiceSideUsersLoader.Load());

    // Register each typed AppSettings class to Section in appsettings.json
    builder.Services.Configure<MinimalApi.App.AppSettings>(config.GetSection("MinimalApi.App.AppSettings"));
    //builder.Services.AddOptions<MinimalApi.App.AppSettings>()
    //    .BindConfiguration(MinimalApi.App.AppSettings.ConfigurationSection)
    //    .ValidateDataAnnotations()
    //    .ValidateOnStart();
    builder.Services.Configure<Stratos.Core.Data.AppSettings>(config.GetSection("Stratos.Core.Data.AppSettings"));
    //builder.Services.AddOptions<Stratos.Core.Data.AppSettings>()
    //    .BindConfiguration(Stratos.Core.Data.AppSettings.ConfigurationSection)
    //    .ValidateDataAnnotations()
    //    .ValidateOnStart();

    // Register DbContexts
    //builder.Services.TryAddTransient<IAuthenticationDataService, AuthenticationDataService>();

    // Register Worker services
    builder.Services.TryAddSingleton<Stratos.Core.WebApi.IAuthenticationService, Stratos.Core.WebApi.AuthenticationService>();

    // Registration for Assemblies
    builder.Services
        .AddApplicationConfiguration()
        .AddInfrastructureConfiguration();

    #endregion

    builder.Services.AddCarter();
    builder.Services.AddMapster();

    var app = builder.Build();

    #region Add Global Exception handler

    // Change to use minimal api pattern at 22:15 in
    // https://www.youtube.com/watch?v=gMwAhKddHYQ&list=PLzYkqgWkHPKBcDIP5gzLfASkQyTdy0t4k&index=4
    // for global unhandled exceptions
    app.Use(async (ctx, next) =>
    {
        try
        {
            await next();
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex.ToString());
            ctx.Response.StatusCode = 500;
            await ctx.Response.WriteAsync("An error has occured, check api log file for details.");
        }
    });

    #endregion

    #region Enable buffering

    // Enables HttpRequest.Body to not be forward-only,
    // so we can log AND process it
    app.Use((context, next) =>
    {
        context.Request.EnableBuffering();
        return next();
    });

    #endregion

    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    #region Add endpoints

    // Create the base url to host
    var group = app.MapGroup("api/configuration/v6")
        .AddEndpointFilter<CallUsageFilter>();

    // Add detailed urls to base group
    group.MapCarter();

    #endregion

    app.Run();
}
catch (Exception ex)
{
    NLog.LogManager.GetCurrentClassLogger().Log(NLog.LogLevel.Error, ex.ToString());
    Environment.ExitCode = 10001;
}

NLog.LogManager.GetCurrentClassLogger().Log(NLog.LogLevel.Info, "Stopping");

if (Environment.UserInteractive)
    Console.ReadKey();