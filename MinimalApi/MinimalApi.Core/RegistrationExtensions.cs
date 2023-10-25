using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MinimalApi.Core;

public static class RegistrationExtensions
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        services.TryAddScoped<IBaseCrudRepo<ApiCallUsage, Guid>, BaseCrudRepo<ApiCallUsage, Guid>>();

        // Register all *Repo classes to DI
        services.Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repo")))
            .AsMatchingInterface()
            .WithScopedLifetime());

        // Register all *Service classes to DI        
        services.Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
            .AsSelf()
            .WithScopedLifetime());
    }
}
