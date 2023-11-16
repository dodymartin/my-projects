using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.WebApis;

public sealed class WebApi : AggregateRoot<WebApiId, int> //EntityBase<WebApi, WebApiId>
{
    private readonly List<WebApiController> _controllers = [];
    private readonly List<WebApiVersion> _versions = [];

    public required ApplicationId ApplicationId { get; set; }
    public required bool UseHttps { get; set; }

    public IReadOnlyList<WebApiController> Controllers => _controllers.AsReadOnly();
    public IReadOnlyList<WebApiVersion> Versions => _versions.AsReadOnly();

    public string? GetBaseUriByApplicationVersion(bool useHttps, string applicationVersion)
    {
        var version = _versions.FirstOrDefault(x => 
            applicationVersion.StartsWith(x.Version[..applicationVersion.Length]));
        if (version is not null)
            return $@"http{(useHttps ? "s" : "")}://+:{version.Port}";
        return default;
    }
}