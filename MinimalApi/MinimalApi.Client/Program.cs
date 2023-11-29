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

        // Add policies to use on calls
        var restBaseAddresses = config.GetSection($"{MinimalApi.Client.AppSettings.ConfigurationSection}:RestBaseAddresses")
            .Get<Dictionary<int, RestBaseAddress>>();
        if (restBaseAddresses is not null)
        {
            foreach (var restBaseAddress in restBaseAddresses)
            {
                // Adds to IHttpClientFactory
                services.AddHttpClient(restBaseAddress.Value.Address, opt =>
                {
                    opt.BaseAddress = new Uri(restBaseAddress.Value.Address);
                });
            }
        }

        #endregion

        services.AddSingleton<IMinimalRestClient, MinimalRestClient>();
        services.AddSingleton<RestService>();

        // Adds to IHttpClientFactory
        services.AddGrpcClient<Greeter.GreeterClient>(o =>
        {
            var addr = config.GetValue<string>("MinimalApi.Client.AppSettings:GrpcBaseAddress")!;
            o.Address = new Uri(addr);
        });
        services.AddSingleton<GrpcService>();

        services.AddHostedService<WorkerService>();
    })
    .Build()
    .StartAsync();

Console.WriteLine("\nDone");
Console.ReadKey(intercept: true);