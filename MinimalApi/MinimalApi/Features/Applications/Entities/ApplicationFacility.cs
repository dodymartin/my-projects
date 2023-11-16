using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.Applications;

public class ApplicationFacility : Entity<ApplicationFacilityId> //EntityBase<ApplicationFacility, ApplicationFacilityId>
{
    public required FacilityId FacilityId { get; set; }
    public required string MinimumAssemblyVersion { get; set; }
}
