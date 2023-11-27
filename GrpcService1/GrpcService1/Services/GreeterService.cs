using Grpc.Core;

namespace GrpcService1.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override async Task StreamHello(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            for (var i = 0; i < 1000000; i++)
            {
                await Task.Delay(50);

                var reply = new HelloReply { Message = $"Hello {request.Name} {i}" };
                await responseStream.WriteAsync(reply);
                if (context.CancellationToken.IsCancellationRequested)
                    break;
            }
        }
    }
}
