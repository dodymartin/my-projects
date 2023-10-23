using MinimalApi.Dom.Common.Models;
using MinimalApi.Dom.WebApis.Entities;
using MinimalApi.Dom.WebApis.ValueObjects;
using ApplicationId = MinimalApi.Dom.Applications.ValueObjects.ApplicationId;

namespace MinimalApi.Dom.WebApis;

public class WebApi : AggregateRoot<WebApiId, int> //EntityBase<WebApi, WebApiId>
{
    private readonly List<WebApiController> _controllers = new();
    private readonly List<WebApiVersion> _versions = new();

    public ApplicationId ApplicationId { get; set; }
    public bool UseHttps { get; set; }

    public IReadOnlyList<WebApiController> Controllers => _controllers.AsReadOnly();
    public IReadOnlyList<WebApiVersion> Versions => _versions.AsReadOnly();

    public string? GetBaseUriByApplicationVersion(bool useHttps, string applicationVersion)
    {
        var version = _versions.FirstOrDefault(x => 
            applicationVersion.StartsWith(x.Version.Substring(0, applicationVersion.Length)));
        if (version is not null)
            return $@"http{(useHttps ? "s" : "")}://+:{version.Port}";
        return default;
    }
}