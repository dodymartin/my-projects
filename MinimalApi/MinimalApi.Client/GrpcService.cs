using System.Net.Http;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Options;
using MinimalApi.Api.Features.Applications;

namespace MinimalApi.Client;

public class GrpcService(IOptions<AppSettings> appSettings, GrpcClientFactory grpcClientFactory)
{
    private readonly AppSettings _appSettings = appSettings.Value;
    private readonly GrpcClientFactory _grpcClientFactory = grpcClientFactory;

    public async Task HandleAsync()
    {
        #region Find active service

        var client = default(Greeter.GreeterClient);
        foreach (var grpcBaseAddress in _appSettings.GrpcBaseAddresses)
        {
            try
            {
                var pingClient = _grpcClientFactory.CreateClient<Greeter.GreeterClient>(grpcBaseAddress.Value.Address);
                var pingResponse = await pingClient.PingAsync(new Empty());
                if (pingResponse.Success)
                {
                    client = pingClient;
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed {grpcBaseAddress.Value.Address}\n\t({ex.Message})");
            }
        }
        if (client is null)
            throw new Exception("Service unavailable");

        #endregion

        Console.WriteLine();
        Console.Write("Who are you ? ");
        var name = Console.ReadLine();

        Console.WriteLine("SayHello to service");

        var request = new HelloRequest { Name = name };
        var response = client.SayHello(request);

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
            using var streamHelloResponse = client.StreamHello(new HelloRequest { Name = name });

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
