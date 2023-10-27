using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.Databases;

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
