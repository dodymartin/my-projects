using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.Applications;

public class ApplicationFacility : Entity<ApplicationFacilityId> //EntityBase<ApplicationFacility, ApplicationFacilityId>
{
    public FacilityId FacilityId { get; set; }
    public string MinimumAssemblyVersion { get; set; }
}
