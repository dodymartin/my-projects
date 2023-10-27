using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.Databases;

public class Facility : AggregateRoot<FacilityId, int> //EntityBase<Facility, FacilityId>
{
    public string Name { get; set; }
}
