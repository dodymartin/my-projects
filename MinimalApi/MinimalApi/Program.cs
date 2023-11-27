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
using MinimalApi.Api;
using MinimalApi.Api.Common;
using MinimalApi.Api.Core;
using MinimalApi.Api.Features.ApiCallUsages;
using MinimalApi.Api.Features.Applications;
using MinimalApi.Api.Features.Databases;
using MinimalApi.Api.Features.WebApis;
using NLog.Extensions.Logging;

_ = bool.TryParse(Environment.GetEnvironmentVariable("DOCKER_RUNNING"), out var isDockerRunning);

if (Environment.UserInteractive && !isDockerRunning)
{
    Console.WriteLine("Application is paused. If needed, attach");
    Console.WriteLine("remote debugger now.");
    Console.WriteLine("\nPress any key to continue!");
    Console.ReadKey();
}

try
{
    #region Get General/NLog Configuration

    if (!isDockerRunning)
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
    if (isDockerRunning)
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

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    #region Add Mediator

    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
        .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    #endregion

    #region Add Database Services

    builder.Services.TryAddScoped<IDbContextSettings, DbContextSettings>();

    builder.Services.AddDbContext<IApiCallUsageDbContext, ApiCallUsageDbContext>();
    builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>();
    builder.Services.AddDbContext<IDatabaseDbContext, DatabaseDbContext>();
    builder.Services.AddDbContext<IWebApiDbContext, WebApiDbContext>();

    builder.Services.TryAddScoped<IBaseCrudRepo<ApiCallUsage, Guid>, BaseCrudRepo<ApiCallUsage, Guid>>();

    #endregion

    builder.Services.TryAddScoped<PublishDomainEventsInterceptor>();

    // Using Scrutor ... Register all *Repo classes to DI
    builder.Services.Scan(scan => scan
        .FromCallingAssembly()
        .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repo")))
        .AsImplementedInterfaces()
        .WithScopedLifetime());

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddGrpc();
    builder.Services.AddGrpcReflection();

    var app = builder.Build();

    #region Enable request buffering

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
        app.MapGrpcReflectionService();
    }

    app.UseHttpsRedirection();
    app.UseRouting();

    #region Just for demo, add docker Container-Id to response header

    _ = app.Use(async (ctx, next) =>
    {
        if (isDockerRunning)
            ctx.Response.Headers.Append("Container-Id", Environment.MachineName);
        await next();
    });

    #endregion

    #region Add Global Exception handler

    /// New IExceptionHandler in NET8
    /// https://www.youtube.com/shorts/jncrUGb03Ac
    /// for global unhandled exceptions
    app.UseExceptionHandler();

    /// Change to use minimal api pattern at 22:15 in
    /// https://www.youtube.com/watch?v=gMwAhKddHYQ&list=PLzYkqgWkHPKBcDIP5gzLfASkQyTdy0t4k&index=4
    /// for global unhandled exceptions, I just refactored into a class
    //app.UseMiddleware<GlobalExceptionMiddleware>();

    #endregion

    //app.UseAuthentication();
    //app.UseAuthorization();

    #region Add endpoints

    // Create the base url to host
    var group = app.MapGroup("api/configuration/v6")
        .AddEndpointFilter<CallUsageFilter>();

    // Add detailed urls to base group
    group.MapCarter();

    // Add detailed grpc urls
    app.MapGrpcService<GreeterService>();

    #endregion

    app.Run();
}
catch (Exception ex)
{
    if (isDockerRunning)
        throw;
    Console.WriteLine(ex.ToString());
    NLog.LogManager.GetCurrentClassLogger().Log(NLog.LogLevel.Error, ex.ToString());
    Environment.ExitCode = 10001;
}

NLog.LogManager.GetCurrentClassLogger().Log(NLog.LogLevel.Info, "Stopping");

if (Environment.UserInteractive && !isDockerRunning)
    Console.ReadKey();