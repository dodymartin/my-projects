using MinimalApi.Dom.Common.Models;
using MinimalApi.Dom.Databases.ValueObjects;
using MinimalApi.Dom.Enumerations;
using MinimalApi.Dom.Facilities;

namespace MinimalApi.Dom.Databases;

public class Database : AggregateRoot<DatabaseId, string> //EntityBase<Database, DatabaseId>
{
    public EnvironmentTypes EnvironmentType { get; set; }
    public DatabaseTypes Type { get; set; }
    public DatabaseSchemaTypes SchemaType { get; set; }
    public string Name { get; set; }
    public DatabaseId ParentId { get; set; }

    private readonly List<Facility> _facilities = new();
    public IReadOnlyList<Facility> Facilities => _facilities.AsReadOnly();
}
