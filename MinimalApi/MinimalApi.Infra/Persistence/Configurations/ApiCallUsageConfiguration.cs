using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinimalApi.Dom.ApiCallUsages;
using MinimalApi.Dom.ApiCallUsages.ValueObjects;

namespace MinimalApi.Infra;

internal class ApiCallUsageConfiguration : IEntityTypeConfiguration<ApiCallUsage>
{
    public void Configure(EntityTypeBuilder<ApiCallUsage> builder)
    {
        builder.ToTable("SVC_CALL_USG", "CMN");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("SVC_CALL_USG_GUID")
                .HasConversion(id => id.Value, value => ApiCallUsageId.Create(value));
        builder.Property(p => p.BasicUsername).HasColumnName("BSIC_USR_NM");
        builder.Property(p => p.Body).HasColumnName("RQST_BLOB").HasColumnType("BLOB");
        builder.Property(p => p.CreateOrigin).HasColumnName("CRT_ORGN");
        //builder.Property(p => p.ElapsedMilliSeconds).HasColumnName("ELPSD_MILLI_SECS");
        builder.Property(p => p.HasAuthorizationHeader).HasColumnName("HAS_AUTH_HDR");
        builder.Property(p => p.ApiApplicationExeName).HasColumnName("SVC_NM");
        builder.Property(p => p.ApiApplicationVersion).HasColumnName("SVC_ASMBLY_VER");
        //builder.Property(p => p.LocalIpAddress).HasColumnName("LCL_IP_ADDR");
        builder.Property(p => p.ApiMachineName).HasColumnName("SVC_MACH_NM");
        //builder.Property(p => p.LocalProcessId).HasColumnName("LCL_PRCS_ID");
        builder.Property(p => p.MethodName).HasColumnName("MTHD_NM");
        builder.Property(p => p.RequestApplicationExeName).HasColumnName("EXE_NM");
        builder.Property(p => p.RequestApplicationVersion).HasColumnName("EXE_VER");
        builder.Property(p => p.RequestIpAddress).HasColumnName("IP_ADDR");
        builder.Property(p => p.RequestMachineName).HasColumnName("MACH_NM");
        builder.Property(p => p.RequestProcessId).HasColumnName("PRCS_ID");
        //builder.Property(p => p.Url).HasColumnName("URL");
    }
}
