using MinimalApi.Dom.Common.Models;
using MinimalApi.Dom.Databases.ValueObjects;
using MinimalApi.Dom.Enumerations;
using MinimalApi.Dom.Facilities.ValueObjects;

namespace MinimalApi.Dom.Databases;

public class Database : AggregateRoot<DatabaseId, string> //EntityBase<Database, DatabaseId>
{
    public EnvironmentTypes EnvironmentType { get; set; }
    public DatabaseTypes Type { get; set; }
    public DatabaseSchemaTypes SchemaType { get; set; }
    public string Name { get; set; }
    public DatabaseId ParentId { get; set; }

    private readonly List<FacilityId> _facilityIds = new();
    public IReadOnlyList<FacilityId> FacilityIds => _facilityIds.AsReadOnly();
}
