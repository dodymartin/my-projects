using Microsoft.EntityFrameworkCore;
using Stratos.Core;

namespace MinimalApi.Dal;

[EntityTypeConfiguration(typeof(ControllerUriFacilityInfoConfiguration))]
public class ControllerUriFacilityInfo : EntityBase<ControllerUriFacilityInfo, Guid>
{
    public override Guid Id { get; set; }

    public virtual string Address { get; set; }
    public virtual EnvironmentTypes EnvironmentType { get; set; }
    public virtual int FacilityId { get; set; }
    public virtual int Order { get; set; }
    public virtual int Port { get; set; }
    public virtual string RedirectVersion { get; set; }
    public virtual string UriName { get; set; }
    public virtual bool UseHttps { get; set; }
    public virtual bool UseProxy { get; set; }
    public virtual string Version { get; set; }
    public virtual int WebApiVersionId { get; set; }
}