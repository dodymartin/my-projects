using Microsoft.Extensions.Options;
using Polly;
using Polly.Wrap;
using RestSharp;

namespace MinimalApi.Client;

public class PolicyHolder(IOptions<AppSettings> appSettings, IHttpClientFactory httpClientFactory) : IPolicyHolder
{
    private const int TIMEOUT = 5;

    private readonly AppSettings _appSettings = appSettings.Value;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public IAsyncPolicy<RestResponse> GetTimeoutPolicy(int timeout = TIMEOUT)
    {
        return Policy.TimeoutAsync<RestResponse>(timeout);
    }

    public IAsyncPolicy<RestResponse> GetRetrySwitchPolicy(Action<IHttpClientFactory, string> setRestClient)
    {
        return
            Policy
                .HandleResult<RestResponse>(r => !r.IsSuccessStatusCode)
                .RetryAsync(3, (response, retryCount) =>
                {
                    Console.WriteLine($"Retry {retryCount} ({response.Result.ErrorMessage})");

                    var httpClientKey = _appSettings.RestBaseAddresses[retryCount].Address;
                    Console.WriteLine($"Switching to {httpClientKey}");
                    setRestClient(_httpClientFactory, httpClientKey);
                });
    }

    public AsyncPolicyWrap<RestResponse> GetTimeoutAndRetrySwitchWrap(Action<IHttpClientFactory, string> setRestClient, int timeout = TIMEOUT)
    {
        _ = setRestClient ?? throw new ArgumentNullException(nameof(setRestClient));

        return Policy.WrapAsync(GetRetrySwitchPolicy(setRestClient), GetTimeoutPolicy(timeout));
    }
}