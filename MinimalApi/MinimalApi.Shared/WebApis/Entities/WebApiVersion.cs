using MinimalApi.Dom.Common.Models;
using MinimalApi.Dom.WebApis.ValueObjects;

namespace MinimalApi.Dom.WebApis.Entities;

public class WebApiVersion : Entity<WebApiVersionId> //EntityBase<WebApiVersion, WebApiVersionId>
{
    public int Port { get; set; }
    public string Version { get; set; }
    public WebApiId WebApiId { get; set; }
}
