using Stratos.Core;

namespace MinimalApi.Core;

public record WebApiVersionId(int Value);

public class WebApiVersion : EntityBase<WebApiVersion, WebApiVersionId>
{
    public override WebApiVersionId Id { get; set; }

    public int Port { get; set; }
    public string Version { get; set; }
    public WebApiId WebApiId { get; set; }
    public WebApi WebApi { get; set; }
}
