using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.Databases;

public sealed class Facility : AggregateRoot<FacilityId, int> //EntityBase<Facility, FacilityId>
{
    public required string Name { get; set; }
}
