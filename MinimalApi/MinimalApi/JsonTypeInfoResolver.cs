using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace MinimalApi;

public class JsonTypeInfoResolver : IJsonTypeInfoResolver
{
    private IServiceProvider ServiceProvider { get; set; }

    public void Initialize(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public JsonTypeInfo? GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        var httpContextAccessor = ServiceProvider.GetRequiredService<IHttpContextAccessor>()
            ?? throw new Exception($"{typeof(IHttpContextAccessor)} is not register in DI container.");

        if (httpContextAccessor.HttpContext.RequestServices.GetService<IServiceProviderIsService>().IsService(type))
            return httpContextAccessor.HttpContext.RequestServices.GetService(type) as JsonTypeInfo;
        return ActivatorUtilities.CreateInstance(httpContextAccessor.HttpContext.RequestServices, type) as JsonTypeInfo;
    }
}
