using Grpc.Core;
using MinimalApi.Api.Features.Applications;

namespace MinimalApi.Client;

public class GrpcService(Greeter.GreeterClient client)
{
    private readonly Greeter.GreeterClient _client = client;

    public async Task HandleAsync()
    {
        Console.WriteLine();
        Console.Write("Who are you ? ");
        var name = Console.ReadLine();

        Console.WriteLine("SayHello to service");

        var request = new HelloRequest { Name = name };
        var response = _client.SayHello(request);

        var foregroundColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(response.Message);
        Console.ForegroundColor = foregroundColor;

        Console.WriteLine("\nContinue with streaming\n");
        Console.ReadKey();

        try
        {
            Console.WriteLine("StreamReply from Service");

            var cts = new CancellationTokenSource();
            using var streamHelloResponse = _client.StreamHello(new HelloRequest { Name = name });

            foregroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            await foreach (var streamedResponse in streamHelloResponse.ResponseStream.ReadAllAsync(cts.Token))
            {
                Console.WriteLine(streamedResponse.Message);
                if (streamedResponse.Message.Contains("99"))
                    cts.Cancel();
            }
        }
        catch (RpcException rpcex) when (rpcex.StatusCode == StatusCode.Cancelled)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(rpcex.Message);
        }
        Console.ForegroundColor = foregroundColor;
    }
}
