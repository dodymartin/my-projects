using System.Threading;
using Microsoft.Extensions.Options;
using Polly;
using RestSharp;

namespace MinimalApi.Client;

public class MinimalRestClient : IMinimalRestClient
{
    private readonly AppSettings _appSettings;
    private readonly ResiliencePipeline<RestResponse> _resiliencePipeline;

    private RestClient? _restClient;

    public MinimalRestClient(IOptions<AppSettings> appSettings, IHttpClientFactory httpClientFactory, /* Version 1*/ IPolicyHolder policyHolder, /* Version 2 */ RetrySwitchStrategyOptions retrySwitchStrategyOptions)
    {
        _appSettings = appSettings.Value;

        // Version 1
        _resiliencePipeline = policyHolder.GetTimeoutAndRetrySwitchResiliencePipeline(SetRestClient);

        // Version 2
        //retrySwitchStrategyOptions.SetRestClientAction = SetRestClient;
        //_resiliencePipeline = new ResiliencePipelineBuilder<RestResponse>()
        //    .AddRetry(retrySwitchStrategyOptions)
        //    .AddTimeout(policyHolder.GetTimeoutStrategy(5))
        //    .Build();

        SetRestClient(httpClientFactory, _appSettings.RestBaseAddresses[0].Address);
    }

    public async Task<string?> GetBaseUriAsync(string body, CancellationToken cancellationToken)
    {
        var url = "base-uri";
        var request = new RestRequest(url).AddJsonBody(body);

        var response = await _resiliencePipeline.ExecuteAsync(
            async token =>
            {
                return await _restClient!.ExecutePostAsync(request, token).ConfigureAwait(false);
            },
            cancellationToken).ConfigureAwait(false);

        return response?.Content;
    }

    private void SetRestClient(IHttpClientFactory httpClientFactory, string httpClientName)
    {
        _restClient?.Dispose();

        _restClient = new RestClient(httpClientFactory.CreateClient(httpClientName));
        _restClient.AddDefaultHeader("X-Host-Name", Environment.MachineName);
        _restClient.AddDefaultHeader("X-User-Domain-Name", Environment.UserDomainName);
        _restClient.AddDefaultHeader("X-User-Name", Environment.UserName);
    }
}