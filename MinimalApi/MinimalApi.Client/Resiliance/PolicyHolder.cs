using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using Polly.Timeout;
using RestSharp;

namespace MinimalApi.Client;

public class PolicyHolder(IOptions<AppSettings> appSettings, IHttpClientFactory httpClientFactory) : IPolicyHolder
{
    private const int MAX_RETRY_ATTEMPTS = 3;
    private const int TIMEOUT = 5;

    private readonly AppSettings _appSettings = appSettings.Value;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public TimeoutStrategyOptions GetTimeoutStrategy(int timeout = TIMEOUT)
    {
        return new TimeoutStrategyOptions
        {
            Timeout = TimeSpan.FromSeconds(timeout),

            OnTimeout = _ =>
            {
                Console.WriteLine("Timeout occurred!");
                return default;
            },
        };
    }

    public RetryStrategyOptions<RestResponse> GetRetryStrategy(Action<IHttpClientFactory, string> setRestClient, int maxRetryAttempts = MAX_RETRY_ATTEMPTS)
    {
        return new RetryStrategyOptions<RestResponse>
        {
            ShouldHandle = new PredicateBuilder<RestResponse>()
                .HandleResult(r => !r.IsSuccessStatusCode),

            OnRetry = args =>
            {
                Console.WriteLine($"Retry {args.AttemptNumber + 1} ({args.Outcome.Result?.ErrorMessage})");

                if (setRestClient is not null && _appSettings.RestBaseAddresses.TryGetValue(args.AttemptNumber + 1, out var restBaseAddress))
                {
                    var httpClientKey = restBaseAddress.Address;
                    Console.WriteLine($"Switching to {httpClientKey}");
                    setRestClient(_httpClientFactory, httpClientKey);
                }
                return default;
            },
            Delay = TimeSpan.FromMilliseconds(1),
            MaxRetryAttempts = maxRetryAttempts
        };
    }

    public ResiliencePipeline<RestResponse> GetTimeoutAndRetrySwitchResiliencePipeline(Action<IHttpClientFactory, string> setRestClient, int maxRetryAttempts = MAX_RETRY_ATTEMPTS, int timeout = TIMEOUT)
    {
        return new ResiliencePipelineBuilder<RestResponse>()
            .AddRetry(GetRetryStrategy(setRestClient))
            .AddTimeout(GetTimeoutStrategy(timeout))
            .Build();
    }
}