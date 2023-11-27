using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MinimalApi.Api.Features.Applications;
using MinimalApi.Client;
using RestSharp;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "production";
var config = new ConfigurationBuilder()
        .SetBasePath(Environment.CurrentDirectory)
        .AddJsonFile("appsettings.json",
            optional: false,
            reloadOnChange: true)
        .AddJsonFile($"appsettings.{environment.ToLower()}.json",
            optional: true,
            reloadOnChange: true)
        //.AddJsonFile("nlog.json",
        //    optional: false,
        //    reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();

await Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration(builder =>
    {
        builder.Sources.Clear();
        builder.AddConfiguration(config);
    })
    .ConfigureServices((services) =>
    {
        // Adds to IHttpClientFactory
        services.AddHttpClient("minimal-rest", o =>
        {
            var addr = config.GetValue<string>("MinimalApi.Api.Client.AppSettings:RestBaseAddress")!;
            o.BaseAddress = new Uri(addr);
        });
        services.AddSingleton<IMinimalRestClient, MinimalRestClient>();
        services.AddSingleton<RestService>();

        // Adds to IHttpClientFactory
        services.AddGrpcClient<Greeter.GreeterClient>(o =>
        {
            var addr = config.GetValue<string>("MinimalApi.Api.Client.AppSettings:GrpcBaseAddress")!;
            o.Address = new Uri(addr);
        });
        services.AddSingleton<GrpcService>();
        
        services.AddHostedService<WorkerService>();
    })
    .Build()
    .StartAsync();

Console.WriteLine("\nDone");
Console.ReadKey(intercept: true);
