using Microsoft.EntityFrameworkCore;
using Stratos.Core;

namespace MinimalApi.Dal;

[EntityTypeConfiguration(typeof(ApplicationConfiguration))]
public class Application : EntityBase<Application, int>
{
    public override int Id { get; set; }

    public string ExeName { get; set; }
    public string FromDirectoryName { get; set; }
    public string MinimumAssemblyVersion { get; set; }
    public string Name { get; set; }

    public List<ApplicationVersion> Versions { get; set; } = new();
}
