using Stratos.Core;

namespace MinimalApi.Core;

public class WebApiController : EntityBase<WebApiController, int>
{
    public override int Id { get; set; }

    public int WebApiId { get; set; }
    public string UriName { get; set; }
}
