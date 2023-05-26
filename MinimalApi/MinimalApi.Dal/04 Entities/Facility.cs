using Microsoft.EntityFrameworkCore;

namespace MinimalApi.Dal;

[EntityTypeConfiguration(typeof(FacilityConfiguration))]
public class Facility
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<Database> Databases { get; } = new();
}
