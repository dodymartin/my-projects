using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.WebApis;

public sealed class WebApiVersion : Entity<WebApiVersionId> //EntityBase<WebApiVersion, WebApiVersionId>
{
    public required int Port { get; set; }
    public required string Version { get; set; }
}
