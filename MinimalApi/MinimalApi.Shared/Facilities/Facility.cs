using MinimalApi.Dom.Common.Models;
using MinimalApi.Dom.Facilities.ValueObjects;

namespace MinimalApi.Dom.Facilities;

public class Facility : AggregateRoot<FacilityId, int> //EntityBase<Facility, FacilityId>
{
    public string Name { get; set; }
}
