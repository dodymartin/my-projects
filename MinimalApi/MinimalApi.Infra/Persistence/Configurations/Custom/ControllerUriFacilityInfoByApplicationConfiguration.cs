//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using MinimalApi.App;

//namespace MinimalApi.Infra
//{
//    internal class ControllerUriFacilityInfoByApplicationConfiguration : IEntityTypeConfiguration<ControllerUriFacilityInfoByApplication>
//    {
//        public void Configure(EntityTypeBuilder<ControllerUriFacilityInfoByApplication> builder)
//        {
//            builder.Property(p => p.Id).HasColumnName("ID");
//            builder.Property(p => p.Address).HasColumnName("ADDR");
//            builder.Property(p => p.ApplicationId).HasColumnName("APLN_ID");
//            builder.Property(p => p.ApplicationName).HasColumnName("APLN_NM");
//            builder.Property(p => p.ApplicationVersion).HasColumnName("APLN_VER");
//            builder.Property(p => p.EnvironmentType).HasColumnName("ENVIR_TP_ID");
//            builder.Property(p => p.FacilityId).HasColumnName("FAC_ID");
//            builder.Property(p => p.Order).HasColumnName("ORD");
//            builder.Property(p => p.Port).HasColumnName("PORT");
//            builder.Property(p => p.UriName).HasColumnName("URI_NM");
//            builder.Property(p => p.UseHttps).HasColumnName("USE_HTTPS");
//            builder.Property(p => p.Version).HasColumnName("VER");
//        }
//    }
//}
