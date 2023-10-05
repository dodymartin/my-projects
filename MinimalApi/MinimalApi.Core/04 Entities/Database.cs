using Stratos.Core;

namespace MinimalApi.Core;

public record DatabaseId(string Value);

public class Database : EntityBase<Database, DatabaseId>
{
    public override DatabaseId Id { get; set; }

    public EnvironmentTypes EnvironmentType { get; set; }
    public DatabaseTypes Type { get; set; }
    public DatabaseSchemaTypes SchemaType { get; set; }
    public string Name { get; set; }
    public DatabaseId ParentId { get; set; }
    public Database Parent { get; set; }

    public List<Facility> Facilities { get; set; }
}
