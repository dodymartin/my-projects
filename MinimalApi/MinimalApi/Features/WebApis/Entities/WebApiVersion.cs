using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.WebApis;

public class WebApiVersion : Entity<WebApiVersionId> //EntityBase<WebApiVersion, WebApiVersionId>
{
    public int Port { get; set; }
    public string Version { get; set; }
}
