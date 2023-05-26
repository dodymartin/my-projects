using Microsoft.EntityFrameworkCore;
using Stratos.Core;

namespace MinimalApi.Dal;

[EntityTypeConfiguration(typeof(ApplicationVersionConfiguration))]
public class ApplicationVersion : EntityBase<ApplicationVersion, int>
{
    public override int Id { get; set; }

    public int ApplicationId { get; set; }
    public string FromDirectoryName { get; set; }
    public string Version { get; set; }
}
