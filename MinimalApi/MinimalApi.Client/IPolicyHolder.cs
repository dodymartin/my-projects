using Polly;
using Polly.Wrap;
using RestSharp;

namespace MinimalApi.Client;

public interface IPolicyHolder
{
    IAsyncPolicy<RestResponse> GetRetrySwitchPolicy(Action<IHttpClientFactory, string> setRestClient);
    IAsyncPolicy<RestResponse> GetTimeoutPolicy(int timeout = 5);
    AsyncPolicyWrap<RestResponse> GetTimeoutAndRetrySwitchWrap(Action<IHttpClientFactory, string> setRestClient, int timeout = 5);
}