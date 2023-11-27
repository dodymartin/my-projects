using RestSharp;

namespace MinimalApi.Client;

public class MinimalRestClient : IMinimalRestClient
{
    private readonly IRestClient _restClient;

    public MinimalRestClient(IHttpClientFactory httpClientFactory)
    {
        _restClient = new RestClient(httpClientFactory.CreateClient("minimal-rest"));
        _restClient.AddDefaultHeader("X-Host-Name", Environment.MachineName);
        _restClient.AddDefaultHeader("X-User-Domain-Name", Environment.UserDomainName);
        _restClient.AddDefaultHeader("X-User-Name", Environment.UserName);
    }

    public async Task<string?> GetBaseUriAsync(string body, CancellationToken cancellationToken)
    {
        var url = "base-uri";
        var request = new RestRequest(url).AddJsonBody(body);
        var response = await _restClient.ExecutePostAsync(request, cancellationToken).ConfigureAwait(false);
        return response?.Content;
    }
}
