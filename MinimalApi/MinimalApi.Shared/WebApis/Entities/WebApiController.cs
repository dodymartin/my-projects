using MinimalApi.Dom.Common.Models;
using MinimalApi.Dom.WebApis.ValueObjects;

namespace MinimalApi.Dom.WebApis.Entities;

public class WebApiController : Entity<WebApiControllerId> //EntityBase<WebApiController, WebApiControllerId>
{
    public string UriName { get; set; }
}
