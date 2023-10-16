using MinimalApi.Dom.Common.Models;
using MinimalApi.Dom.WebApis.ValueObjects;

namespace MinimalApi.Dom.WebApis.Entities;

public class WebApiController : Entity<WebApiControllerId> //EntityBase<WebApiController, WebApiControllerId>
{
    public WebApiId WebApiId { get; set; }
    public string UriName { get; set; }
}
