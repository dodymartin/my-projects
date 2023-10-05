using Stratos.Core;

namespace MinimalApi.Core;

public record ApplicationId(int Value);

public class Application : EntityBase<Application, ApplicationId>
{
    public override ApplicationId Id { get; set; }

    public string ExeName { get; set; }
    public string FromDirectoryName { get; set; }
    public string? MinimumAssemblyVersion { get; set; }
    public string Name { get; set; }

    public IEnumerable<ApplicationVersion> Versions { get; set; } = new List<ApplicationVersion>();
}
