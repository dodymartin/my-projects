using Polly;
using Polly.Retry;
using RestSharp;

namespace MinimalApi.Client;

public class RetrySwitchStrategyOptions : RetryStrategyOptions<RestResponse>
{
    private readonly IDictionary<int, RestBaseAddress> _restBaseAddresses;
    private readonly IHttpClientFactory _httpClientFactory;

    public Action<IHttpClientFactory, string>? SetRestClientAction { get; set; }

    public RetrySwitchStrategyOptions(IDictionary<int, RestBaseAddress> restBaseAddresses, IHttpClientFactory httpClientFactory)
    {
        _restBaseAddresses = restBaseAddresses;
        _httpClientFactory = httpClientFactory;

        Delay = TimeSpan.FromMilliseconds(1);
        MaxRetryAttempts = restBaseAddresses.Count - 1;

        ShouldHandle = new PredicateBuilder<RestResponse>()
            .HandleResult(r => !r.IsSuccessStatusCode);

        OnRetry = args =>
        {
            Console.WriteLine($"Retry {args.AttemptNumber + 1} ({args.Outcome.Result?.ErrorMessage})");

            if (SetRestClientAction is not null && _restBaseAddresses.TryGetValue(args.AttemptNumber + 1, out var restBaseAddress))
            {
                var httpClientKey = restBaseAddress.Address;
                Console.WriteLine($"Switching to {httpClientKey}");
                SetRestClientAction(_httpClientFactory, httpClientKey);
            }
            return default;
        };
    }
}
