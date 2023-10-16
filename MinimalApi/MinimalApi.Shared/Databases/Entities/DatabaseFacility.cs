using MinimalApi.Dom.Databases.ValueObjects;
using MinimalApi.Dom.Facilities.ValueObjects;

namespace MinimalApi.Dom.Databases.Entities;

public class DatabaseFacility
{
    public DatabaseId DatabaseId { get; set; }
    public FacilityId FacilityId { get; set; }
}
