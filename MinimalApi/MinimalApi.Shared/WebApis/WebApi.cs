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
}
