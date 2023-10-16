using MinimalApi.Dom.Applications.Entities;
using MinimalApi.Dom.Common.Models;
using ApplicationId = MinimalApi.Dom.Applications.ValueObjects.ApplicationId;

namespace MinimalApi.Dom.Applications;

public class Application : AggregateRoot<ApplicationId, int> //EntityBase<Application, ApplicationId>
{
    public string ExeName { get; set; }
    public string FromDirectoryName { get; set; }
    public string? MinimumAssemblyVersion { get; set; }
    public string Name { get; set; }

    private readonly List<ApplicationFacility> _facilities = new();
    public IReadOnlyList<ApplicationFacility> Facilities => _facilities.AsReadOnly();

    private readonly List<ApplicationVersion> _versions = new();
    public IReadOnlyList<ApplicationVersion> Versions => _versions.AsReadOnly();
}
