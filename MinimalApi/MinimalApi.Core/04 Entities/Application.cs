using Stratos.Core;

namespace MinimalApi.Core;

public class Application : EntityBase<Application, int>
{
    public override int Id { get; set; }

    public string ExeName { get; set; }
    public string FromDirectoryName { get; set; }
    public string? MinimumAssemblyVersion { get; set; }
    public string Name { get; set; }

    public IEnumerable<ApplicationVersion> Versions { get; set; } = new List<ApplicationVersion>();
}
