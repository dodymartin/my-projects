//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using MinimalApi.App;

//namespace MinimalApi.Infra
//{
//    internal class ControllerUriInfoConfiguration : IEntityTypeConfiguration<ControllerUriInfo>
//    {
//        public void Configure(EntityTypeBuilder<ControllerUriInfo> builder)
//        {
//            builder.Property(p => p.Id).HasColumnName("ID");
//            builder.Property(p => p.Address).HasColumnName("ADDR");
//            builder.Property(p => p.EnvironmentType).HasColumnName("ENVIR_TP_ID");
//            builder.Property(p => p.Order).HasColumnName("ORD");
//            builder.Property(p => p.Port).HasColumnName("PORT");
//            builder.Property(p => p.RedirectVersion).HasColumnName("RDRCT_VER");
//            builder.Property(p => p.UriName).HasColumnName("URI_NM");
//            builder.Property(p => p.UseHttps).HasColumnName("USE_HTTPS");
//            builder.Property(p => p.UseProxy).HasColumnName("USE_PROXY");
//            builder.Property(p => p.Version).HasColumnName("VER");
//            builder.Property(p => p.WebApiVersionId).HasColumnName("WEB_API_VER_ID");
//        }
//    }
//}
