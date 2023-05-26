using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MinimalApi.Dal
{
    internal class WebApiControllerConfiguration : IEntityTypeConfiguration<WebApiController>
    {
        public void Configure(EntityTypeBuilder<WebApiController> builder)
        {
            builder.ToTable("WEB_API_CTLR", "CMN_MSTR");

            builder.Property(p => p.Id).HasColumnName("WEB_API_CTLR_ID");
            builder.Property(p => p.UriName).HasColumnName("URI_NM");
            builder.Property(p => p.WebApiId).HasColumnName("WEB_API_ID");
        }
    }
}
