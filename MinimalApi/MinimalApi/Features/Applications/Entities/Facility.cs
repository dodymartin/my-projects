using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.Applications;

public class Facility : AggregateRoot<FacilityId, int> //EntityBase<Facility, FacilityId>
{
    public string Name { get; set; }
}
