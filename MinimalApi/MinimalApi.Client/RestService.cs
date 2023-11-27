using RestSharp;

namespace MinimalApi.Client;

public class RestService(IMinimalRestClient minimalRestClient)
{
    private readonly IMinimalRestClient _minimalRestClient = minimalRestClient;

    public async Task HandleAsync()
    {
        var body = """
        {
            "applicationId": 249,
            "applicationVersion": "3.2.0.0"
        }
        """;

        var cts = new CancellationTokenSource();
        var baseUri = await _minimalRestClient.GetBaseUriAsync(body, cts.Token);
        var foregroundColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(baseUri);
        Console.ForegroundColor = foregroundColor;
    }
}
