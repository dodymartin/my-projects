using Polly;
using Polly.Retry;
using Polly.Timeout;
using RestSharp;
namespace MinimalApi.Client;

public interface IPolicyHolder
{
    RetryStrategyOptions<RestResponse> GetRetryStrategy(Action<IHttpClientFactory, string> setRestClient, int maxRetryAttempts = 3);
    ResiliencePipeline<RestResponse> GetTimeoutAndRetrySwitchResiliencePipeline(Action<IHttpClientFactory, string> setRestClient, int maxRetryAttempts = 3, int timeout = 5);
    TimeoutStrategyOptions GetTimeoutStrategy(int timeout = 5);
}