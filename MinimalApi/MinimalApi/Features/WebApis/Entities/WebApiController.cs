using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.WebApis;

public sealed class WebApiController : Entity<WebApiControllerId> //EntityBase<WebApiController, WebApiControllerId>
{
    public required string UriName { get; set; }
}
