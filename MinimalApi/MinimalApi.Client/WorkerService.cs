using Microsoft.Extensions.Hosting;

namespace MinimalApi.Client;

public class WorkerService(RestService restService, GrpcService grpcService) : IHostedService
{
    private readonly RestService _restService = restService;
    private readonly GrpcService _grpcService = grpcService;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        ConsoleKeyInfo key;
        do
        {
            Console.WriteLine();
            Console.WriteLine("1. Test REST endpoints");
            Console.WriteLine("2. Test gRPC endpoints");
            Console.WriteLine("3. Quit\n");
            key = Console.ReadKey(intercept: true);

            switch (key.KeyChar)
            {
                case '1':
                    await _restService.HandleAsync();
                    break;
                case '2':
                    await _grpcService.HandleAsync();
                    break;
                default:
                    break;
            }

        } while (key.KeyChar != '3');
    }

    public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
}
