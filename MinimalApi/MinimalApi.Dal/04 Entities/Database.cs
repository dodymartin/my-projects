using System.Security.Policy;
using Microsoft.EntityFrameworkCore;
using Stratos.Core;

namespace MinimalApi.Dal;

[EntityTypeConfiguration(typeof(DatabaseConfiguration))]
public class Database : EntityBase<Database, string>
{
    public override string Id { get; set; }

    public EnvironmentTypes EnvironmentType { get; set; }
    public DatabaseTypes Type { get; set; }
    public DatabaseSchemaTypes SchemaType { get; set; }
    public string Name { get; set; }
    public string ParentId { get; set; }
    public Database Parent { get; set; }

    public List<Facility> Facilities { get; set; }
}
