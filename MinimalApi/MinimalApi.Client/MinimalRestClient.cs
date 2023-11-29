using Microsoft.Extensions.Options;
using Polly.Wrap;
using RestSharp;

namespace MinimalApi.Client;

public class MinimalRestClient : IMinimalRestClient
{
    private readonly AppSettings _appSettings;
    private readonly AsyncPolicyWrap<RestResponse> _policyWrap;

    private RestClient? _restClient;

    public MinimalRestClient(IOptions<AppSettings> appSettings, IHttpClientFactory httpClientFactory, IPolicyHolder policyHolder)
    {
        _appSettings = appSettings.Value;

        _policyWrap = policyHolder.GetTimeoutAndRetrySwitchWrap(SetRestClient);
        SetRestClient(httpClientFactory, _appSettings.RestBaseAddresses[0].Address);
    }

    public async Task<string?> GetBaseUriAsync(string body, CancellationToken cancellationToken)
    {
        var url = "base-uri";
        var request = new RestRequest(url).AddJsonBody(body);

        var response = await _policyWrap.ExecuteAsync(
            token => _restClient!.ExecutePostAsync(request, token), cancellationToken)
            .ConfigureAwait(false);

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