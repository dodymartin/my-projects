// See https://aka.ms/new-console-template for more information
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer1;
using GrpcService1;

Console.Write("Who are you ? ");
var name = Console.ReadLine();

Console.WriteLine("SayHello to service");
var channel = GrpcChannel.ForAddress("https://localhost:7070");
var client = new Greeter.GreeterClient(channel);

var sayHelloResponse = client.SayHello(new HelloRequest { Name = name });
Console.WriteLine(sayHelloResponse.Message);
Console.WriteLine("\nContinue with streaming\n");
Console.ReadKey();

try
{
    Console.WriteLine("StreamReply from Service");

    var cts = new CancellationTokenSource();
    using var helloResponse = client.StreamHello(new HelloRequest { Name = name });

    await foreach (var response in helloResponse.ResponseStream.ReadAllAsync(cts.Token))
    { 
        Console.WriteLine(response.Message);
        if (response.Message.Contains("99"))
            cts.Cancel();
    }

}
catch (RpcException rpcex) when (rpcex.StatusCode == StatusCode.Cancelled)
{
    Console.WriteLine(rpcex.Message);
}

Console.ReadKey();
