using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MinimalApi.App;
using MinimalApi.App.Interfaces;
using MinimalApi.Dom.ApiCallUsages;
using MinimalApi.Infra.Persistence;
using MinimalApi.Infra.Persistence.Interceptors;

namespace MinimalApi.Infra;

public static class RegistrationExtensions
{
    public static IServiceCollection AddInfrastructureConfiguration(this IServiceCollection services)
    {
        services.TryAddScoped<IBaseCrudRepo<ApiCallUsage, Guid>, BaseCrudRepo<ApiCallUsage, Guid>>();

        services.TryAddScoped<PublishDomainEventsInterceptor>();
        services.TryAddScoped<IDbContextSettings, DbContextSettings>();
        services.AddDbContext<IMinimalApiDbContext, MinimalApiDbContext>();

        // Register all *Repo classes to DI
        services.Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repo")))
            .AsMatchingInterface()
            .WithScopedLifetime());

        return services;
    }
}