using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MinimalApi.App;
using MinimalApi.App.Interfaces;
using MinimalApi.Dom.ApiCallUsages;
using MinimalApi.Infra.Persistence;

namespace MinimalApi.Infra;

public static class RegistrationExtensions
{
    public static void AddInfrastructureConfiguration(this IServiceCollection services)
    {
        services.TryAddScoped<IBaseCrudRepo<ApiCallUsage, Guid>, BaseCrudRepo<ApiCallUsage, Guid>>();

        services.TryAddScoped<IDbContextSettings, DbContextSettings>();
        services.TryAddScoped<IMinimalApiDbContext, MinimalApiDbContext>();
    }
}