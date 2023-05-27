using Microsoft.EntityFrameworkCore;
using Stratos.Core;

namespace MinimalApi.Dal;

[EntityTypeConfiguration(typeof(ControllerUriInfoByApplicationConfiguration))]
public class ControllerUriInfoByApplication : EntityBase<ControllerUriInfoByApplication, Guid>
{
    public override Guid Id { get; set; }

    public virtual string Address { get; set; }
    public virtual int ApplicationId { get; set; }
    public virtual string ApplicationName { get; set; }
    public virtual string ApplicationVersion { get; set; }
    public virtual EnvironmentTypes EnvironmentType { get; set; }
    public virtual string? MachineName { get; set; }
    public virtual int Order { get; set; }
    public virtual int Port { get; set; }
    public virtual string UriName { get; set; }
    public virtual bool UseHttps { get; set; }
    public virtual string Version { get; set; }
}