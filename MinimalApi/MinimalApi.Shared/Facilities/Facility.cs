using MinimalApi.Dom.Common.Models;
using MinimalApi.Dom.Databases;
using MinimalApi.Dom.Facilities.ValueObjects;

namespace MinimalApi.Dom.Facilities;

public class Facility : AggregateRoot<FacilityId, int> //EntityBase<Facility, FacilityId>
{
    public string Name { get; set; }

    private readonly List<Database> _databases = new();
    public IReadOnlyList<Database> Databases => _databases.AsReadOnly();
}
