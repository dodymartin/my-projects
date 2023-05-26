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
    public virtual string MachineName {get;set;}
    public virtual int Order { get; set; }
    public virtual int Port { get; set; }
    public virtual string UriName { get; set; }
    public virtual bool UseHttps { get; set; }
    public virtual string Version { get; set; }
}

//public sealed class ControllerUriInfoByApplicationMap : ClassMap<ControllerUriInfoByApplication>
//{
//    public ControllerUriInfoByApplicationMap()
//    {
//        ReadOnly();

//        Id(x => x.Id, "ID");

//        Map(x => x.Address, "ADDR");
//        Map(x => x.ApplicationId, "APLN_ID");
//        Map(x => x.ApplicationName, "APLN_NM");
//        Map(x => x.ApplicationVersion, "APLN_VER");
//        Map(x => x.EnvironmentType, "ENVIR_TP_ID")
//            .CustomType<EnvironmentTypes>();
//        Map(x => x.MachineName, "MACH_NM");
//        Map(x => x.Order, "ORD");
//        Map(x => x.Port, "PORT");
//        Map(x => x.UriName, "URI_NM");
//        Map(x => x.UseHttps, "USE_HTTPS");
//        Map(x => x.Version, "VER");
//    }
//}