using Stratos.Core;

namespace MinimalApi.Core;

public record WebApiControllerId(int Value);

public class WebApiController : EntityBase<WebApiController, WebApiControllerId>
{
    public override WebApiControllerId Id { get; set; }

    public WebApiId WebApiId { get; set; }
    public string UriName { get; set; }
}
