using Stratos.Core;

namespace MinimalApi.Core;

public record WebApiId(int Value);

public class WebApi : EntityBase<WebApi, WebApiId>
{
    public override WebApiId Id { get; set; }

    public ApplicationId ApplicationId { get; set; }
    public bool UseHttps { get; set; }
}
