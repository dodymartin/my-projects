using Stratos.Core;

namespace MinimalApi.Core;

public record ApplicationVersionId(int Value);

public class ApplicationVersion : EntityBase<ApplicationVersion, ApplicationVersionId>
{
    public override ApplicationVersionId Id { get; set; }

    public ApplicationId ApplicationId { get; set; }
    public string FromDirectoryName { get; set; }
    public string Version { get; set; }
}
