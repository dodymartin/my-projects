using MinimalApi.Dom.Applications.ValueObjects;
using MinimalApi.Dom.Common.Models;
using MinimalApi.Dom.Facilities.ValueObjects;

namespace MinimalApi.Dom.Applications.Entities;

public class ApplicationFacility : Entity<ApplicationFacilityId> //EntityBase<ApplicationFacility, ApplicationFacilityId>
{
    public FacilityId FacilityId { get; set; }
    public string MinimumAssemblyVersion { get; set; }
}
