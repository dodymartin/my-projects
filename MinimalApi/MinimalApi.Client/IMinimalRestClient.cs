namespace MinimalApi.Client;

public interface IMinimalRestClient
{
    Task<string?> GetBaseUriAsync(string body, CancellationToken cancellationToken);
}