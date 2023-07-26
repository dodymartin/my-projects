using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MinimalApi.Core;

namespace MinimalApi.Infra;

public static class RegistrationExtensions
{
    public static void AddInfraServices(this IServiceCollection services)
    {
        services.TryAddScoped<IDbContextSettings, DbContextSettings>();
        services.TryAddScoped<IMinimalApiDbContext, MinimalApiDbContext>();
    }
}