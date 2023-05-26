using Microsoft.EntityFrameworkCore;

namespace MinimalApi.Dal;

[EntityTypeConfiguration(typeof(DatabaseFacilityConfiguration))]
public class DatabaseFacility
{
    public string DatabaseId { get; set; }
    public int FacilityId { get; set; }
}
