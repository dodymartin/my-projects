using MinimalApi.Dom.Applications.ValueObjects;
using MinimalApi.Dom.Common.Models;
using ApplicationId = MinimalApi.Dom.Applications.ValueObjects.ApplicationId;

namespace MinimalApi.Dom.Applications.Entities;

public class ApplicationVersion : Entity<ApplicationVersionId> //EntityBase<ApplicationVersion, ApplicationVersionId>
{
    public string FromDirectoryName { get; set; }
    public string Version { get; set; }
}
