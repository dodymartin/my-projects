using Microsoft.EntityFrameworkCore;
using Stratos.Core;

namespace MinimalApi.Dal;

[EntityTypeConfiguration(typeof(ApplicationFacilityConfiguration))]
public class ApplicationFacility : EntityBase<ApplicationFacility, int>
{
    public override int Id { get; set; }

    public int ApplicationId { get; set; }
    public Application Application { get; set; }
    public int FacilityId { get; set; }
    public Facility Facility { get; set; }
    public string MinimumAssemblyVersion { get; set; }
}
