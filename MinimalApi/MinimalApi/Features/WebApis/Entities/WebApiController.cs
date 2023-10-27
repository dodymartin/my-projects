using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.WebApis;

public class WebApiController : Entity<WebApiControllerId> //EntityBase<WebApiController, WebApiControllerId>
{
    public string UriName { get; set; }
}
