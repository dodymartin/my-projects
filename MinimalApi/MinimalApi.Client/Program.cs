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
        services.AddSingleton<IPolicyHolder, PolicyHolder>();

        services.AddOptions<MinimalApi.Client.AppSettings>()
            .BindConfiguration(MinimalApi.Client.AppSettings.ConfigurationSection);

        #region Add HttpClients to HttpClientFactory

        var restBaseAddresses = config.GetSection($"{MinimalApi.Client.AppSettings.ConfigurationSection}:RestBaseAddresses")
            .Get<Dictionary<int, RestBaseAddress>>();
        if (restBaseAddresses is not null)
        {
            foreach (var restBaseAddress in restBaseAddresses)
            {
                services.AddHttpClient(restBaseAddress.Value.Address, opt =>
                {
                    opt.BaseAddress = new Uri(restBaseAddress.Value.Address);
                });
            }
        }

        #endregion

        services.AddSingleton<IMinimalRestClient, MinimalRestClient>();
        services.AddSingleton<RestService>();

        #region Add GrpcClients to GrpcClientFactory

        var grpcBaseAddresses = config.GetSection($"{MinimalApi.Client.AppSettings.ConfigurationSection}:GrpcBaseAddresses")
            .Get<Dictionary<int, RestBaseAddress>>();
        if (grpcBaseAddresses is not null)
        {
            foreach (var grpcBaseAddress in grpcBaseAddresses)
            {
                services.AddGrpcClient<Greeter.GreeterClient>(grpcBaseAddress.Value.Address, opt =>
                {
                    opt.Address = new Uri(grpcBaseAddress.Value.Address);
                });
            }
        }

        #endregion

        //// Adds to GrpcClientFactory
        //services.AddGrpcClient<Greeter.GreeterClient>(o =>
        //{
        //    var addr = config.GetValue<string>("MinimalApi.Client.AppSettings:GrpcBaseAddress")!;
        //    o.Address = new Uri(addr);
        //});
        services.AddSingleton<GrpcService>();

        services.AddHostedService<WorkerService>();
    })
    .Build()
    .StartAsync();

Console.WriteLine("\nDone");
Console.ReadKey(intercept: true);