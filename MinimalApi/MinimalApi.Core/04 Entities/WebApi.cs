using Stratos.Core;

namespace MinimalApi.Core;

public class WebApi : EntityBase<WebApi, int>
{
    public override int Id { get; set; }

    public int ApplicationId { get; set; }
    public bool UseHttps { get; set; }
}
