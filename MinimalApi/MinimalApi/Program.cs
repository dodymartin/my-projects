using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MinimalApi.Api.Common;
using MinimalApi.Api.Core;
using MinimalApi.Api.Features.ApiCallUsages;
using MinimalApi.Api.Features.Applications;
using MinimalApi.Api.Features.Databases;
using MinimalApi.Api.Features.WebApis;
using NLog.Extensions.Logging;

bool.TryParse(Environment.GetEnvironmentVariable("DOCKER_RUNNING"), out var dockerRunning);

if (Environment.UserInteractive && !dockerRunning)
{
    Console.WriteLine("Application is paused. If needed, attach");
    Console.WriteLine("remote debugger now.");
    Console.WriteLine("\nPress any key to continue!");
    Console.ReadKey();
}

try
{
    #region Get General/NLog Configuration

    if (!dockerRunning)
        Environment.CurrentDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName)!;

    Console.WriteLine($"Current Directory        : {Environment.CurrentDirectory}");
    Console.WriteLine($"Base Directory (for logs): {AppDomain.CurrentDomain.BaseDirectory}");

    var configPath = Environment.GetEnvironmentVariable("CONFIG_PATH") ?? Environment.CurrentDirectory;
    var configBySitePath = Environment.GetEnvironmentVariable("CONFIG_SITE_PATH") ?? string.Empty;

    // Load IConfiguration from .json files
    var config = CoreMethods.BuildConfiguration(configPath).Build();

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

    //// Comes from Stratos.Core.WebApi
    ////
    //// Add Basic Authorization
    //builder.Services.AddAuthentication()
    //    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicDefaults.AuthenticationScheme, null);
    //builder.Services.AddAuthorizationBuilder()
    //    .AddPolicy(BasicDefaults.AuthenticationScheme, policy =>
    //    {
    //        policy.RequireAuthenticatedUser()
    //            .AuthenticationSchemes.Add(BasicDefaults.AuthenticationScheme);
    //    });

    //// Add JWT Authorization
    //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //    .AddJwtBearer(opts =>
    //    {
    //        opts.SaveToken = true;
    //        opts.TokenValidationParameters = JwtConfiguration.GetTokenValidationParameters();
    //    });
    //builder.Services.AddAuthorization();

    #endregion

    #region Add Services

    // TODO: From AddBootstrapServices, create new methods
    //  AddCoreLoaderServices and AddCoreHttpServices, do not
    //  include AuthenticationWebApi and ConfigurationWebApi

    //builder.Services.AddBootstrapServices();

    //// Comes from Stratos.Core
    //builder.Services.TryAddSingleton(ClientSideUsersLoader.Load(configPath, sitePath));
    if (dockerRunning)
        builder.Services.TryAddSingleton(DatabasesLoader.Load(configPath, configBySitePath));
    else
        builder.Services.TryAddSingleton(DatabasesLoader.Load(configPath));
    //builder.Services.TryAddSingleton(SiteConfigurationLoader.Load(configPath, sitePath));

    //// Register IApplicationContext to force feeding ApplicationId
    //builder.Services.TryAddSingleton<IApplicationContext>(sp =>
    //{
    //    var logger = sp.GetRequiredService<ILogger<ApplicationContext>>();
    //    return new ApplicationContext(
    //        logger,
    //        (int)Applications.StratosConfigurationWebApiHost);
    //});

    //// Register IServiceSideUsers (serviceSideUsers.json)
    //builder.Services.TryAddSingleton(ServiceSideUsersLoader.Load());

    // Register each typed AppSettings class to Section in appsettings.json
    builder.Services.AddOptions<MinimalApi.Api.AppSettings>()
        .BindConfiguration(MinimalApi.Api.AppSettings.ConfigurationSection)
        .ValidateDataAnnotations()
        .ValidateOnStart();

    #endregion

    builder.Services.AddCarter();
    builder.Services.AddMapster();

    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
        .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    builder.Services.TryAddScoped<PublishDomainEventsInterceptor>();
    builder.Services.TryAddScoped<IDbContextSettings, DbContextSettings>();
    builder.Services.AddDbContext<IApiCallUsageDbContext, ApiCallUsageDbContext>();
    builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>();
    builder.Services.AddDbContext<IDatabaseDbContext, DatabaseDbContext>();
    builder.Services.AddDbContext<IWebApiDbContext, WebApiDbContext>();

    builder.Services.TryAddScoped<IBaseCrudRepo<ApiCallUsage, Guid>, BaseCrudRepo<ApiCallUsage, Guid>>();

    // Using Scrutor ... Register all *Repo classes to DI
    builder.Services.Scan(scan => scan
        .FromCallingAssembly()
        .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repo")))
        .AsMatchingInterface()
        .WithScopedLifetime());

    //// Using Scrutor ... Register all *Service classes to DI        
    //builder.Services.Scan(scan => scan
    //    .FromCallingAssembly()
    //    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
    //    .AsSelf()
    //    .WithScopedLifetime());

    //// Using Scrutor ... Register all *Validator classes to DI
    //builder.Services.Scan(scan => scan
    //    .FromCallingAssembly()
    //    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Validator")))
    //    .AsSelf()
    //    .WithSingletonLifetime());

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    #region Add Global Exception handler

    // Change to use minimal api pattern at 22:15 in
    // https://www.youtube.com/watch?v=gMwAhKddHYQ&list=PLzYkqgWkHPKBcDIP5gzLfASkQyTdy0t4k&index=4
    // for global unhandled exceptions
    app.Use(async (ctx, next) =>
    {
        try
        {
            if (dockerRunning)
                ctx.Response.Headers.Add("Container-Id", Environment.MachineName);
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

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseRouting();

    //app.UseAuthentication();
    //app.UseAuthorization();

    #region Add endpoints

    // Create the base url to host
    var group = app.MapGroup("api/configuration/v8")
        .AddEndpointFilter<CallUsageFilter>();

    // Add detailed urls to base group
    group.MapCarter();

    #endregion

    app.Run();
}
catch (Exception ex)
{
    if (dockerRunning)
        throw;
    Console.WriteLine(ex.ToString());
    NLog.LogManager.GetCurrentClassLogger().Log(NLog.LogLevel.Error, ex.ToString());
    Environment.ExitCode = 10001;

}

NLog.LogManager.GetCurrentClassLogger().Log(NLog.LogLevel.Info, "Stopping");

if (Environment.UserInteractive && !dockerRunning)
    Console.ReadKey();