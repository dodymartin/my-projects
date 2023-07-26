using Stratos.Core;

namespace MinimalApi.Core;

public class WebApiVersion : EntityBase<WebApiVersion, int>
{
    public override int Id { get; set; }

    public int Port { get; set; }
    public string Version { get; set; }
    public int WebApiId { get; set; }
    public WebApi WebApi { get; set; }
}
