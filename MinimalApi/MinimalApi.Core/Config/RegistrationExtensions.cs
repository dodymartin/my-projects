using Microsoft.Extensions.DependencyInjection;

namespace MinimalApi.App;

public static class RegistrationExtensions
{
    public static void AddApplicationConfiguration(this IServiceCollection services)
    {
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
