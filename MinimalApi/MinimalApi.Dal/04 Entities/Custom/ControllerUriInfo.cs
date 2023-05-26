using Microsoft.EntityFrameworkCore;
using Stratos.Core;

namespace MinimalApi.Dal;

[EntityTypeConfiguration(typeof(ControllerUriInfoConfiguration))]
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
}

//public sealed class ControllerUriInfoMap : ClassMap<ControllerUriInfo>
//{
//    public ControllerUriInfoMap()
//    {
//        ReadOnly();

//        Id(x => x.Id, "ID");

//        Map(x => x.Address, "ADDR");
//        Map(x => x.EnvironmentType, "ENVIR_TP_ID")
//            .CustomType<EnvironmentTypes>();
//        Map(x => x.Order, "ORD");
//        Map(x => x.Port, "PORT");
//        Map(x => x.RedirectVersion, "RDRCT_VER");
//        Map(x => x.UriName, "URI_NM");
//        Map(x => x.UseHttps, "USE_HTTPS");
//        Map(x => x.UseProxy, "USE_PROXY");
//        Map(x => x.Version, "VER");
//        Map(x => x.WebApiVersionId, "WEB_API_VER_ID");
//    }
//}