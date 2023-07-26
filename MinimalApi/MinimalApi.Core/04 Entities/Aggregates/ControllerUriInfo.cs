using MinimalApi.Shared;
using Stratos.Core;

namespace MinimalApi.Core;

public class ControllerUriInfo : EntityBase<ControllerUriInfo, Guid>
{
    public override Guid Id { get; set; }

    public virtual string Address { get; set; }
    public virtual EnvironmentTypes EnvironmentType { get; set;}
    public virtual int Order { get; set; }
    public virtual int Port { get; set; }
    public virtual string RedirectVersion { get; set; }
    public virtual string UriName { get; set; }
    public virtual bool UseHttps { get; set; }
    public virtual bool UseProxy { get; set; }
    public virtual string Version { get; set; }
    public virtual int WebApiVersionId { get; set; }

    #region Cast Operators

    public static explicit operator ControllerUriResponse(ControllerUriInfo from)
    {
        return new ControllerUriResponse
        {
            Address = from.Address,
            Order = from.Order,
            Port = from.Port,
            UriName = from.UriName,
            UseHttps = from.UseHttps,
            Version = from.RedirectVersion
        };
    }

    #endregion
}