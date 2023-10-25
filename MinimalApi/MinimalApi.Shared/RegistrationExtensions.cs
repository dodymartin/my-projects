using Microsoft.Extensions.DependencyInjection;

namespace MinimalApi.Shared;

public static class RegistrationExtensions
{
    public static void AddSharedServices(this IServiceCollection services)
    {
        // Register all *Validator classes to DI
        services.Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Validator")))
            .AsSelf()
            .WithSingletonLifetime());
    }
}
