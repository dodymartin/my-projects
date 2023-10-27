using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.Applications;

public class ApplicationVersion : Entity<ApplicationVersionId> //EntityBase<ApplicationVersion, ApplicationVersionId>
{
    public string FromDirectoryName { get; set; }
    public string Version { get; set; }
}
