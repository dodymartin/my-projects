using Grpc.Core;

namespace GrpcService1.Services
{
    public class TesterService : Tester.TesterBase
    {
        private readonly ILogger<TesterService> _logger;
        public TesterService(ILogger<TesterService> logger)
        {
            _logger = logger;
        }

        public override Task<TestHelloReply> SayTestHello(TestHelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new TestHelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
