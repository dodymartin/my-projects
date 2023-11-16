using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.Applications;

public sealed class ApplicationVersion : Entity<ApplicationVersionId> //EntityBase<ApplicationVersion, ApplicationVersionId>
{
    public required string FromDirectoryName { get; set; }
    public required string Version { get; set; }
}
