using Grpc.Core;

namespace MinimalApi.Api.Features.Applications;

public class GreeterService(ILogger<GreeterService> logger) : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger = logger;
    private readonly Guid _id = Guid.NewGuid();

    public override Task<PingReply> Ping(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new PingReply
        {
            Success = true
        });
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = $"Hello {request.Name}, {_id}"
        });
    }

    public override async Task StreamHello(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
    {
        for (var i = 0; i < 1000000; i++)
        {
            await Task.Delay(50);

            var reply = new HelloReply { Message = $"Hello {request.Name} {i}, {_id}" };
            await responseStream.WriteAsync(reply);
            if (context.CancellationToken.IsCancellationRequested)
                break;
        }
    }
}
