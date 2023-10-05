using Stratos.Core;

namespace MinimalApi.Core;

public record ApplicationFacilityId(int Value);

public class ApplicationFacility : EntityBase<ApplicationFacility, ApplicationFacilityId>
{
    public override ApplicationFacilityId Id { get; set; }

    public ApplicationId ApplicationId { get; set; }
    public Application Application { get; set; }
    public FacilityId FacilityId { get; set; }
    public Facility Facility { get; set; }
    public string MinimumAssemblyVersion { get; set; }
}
