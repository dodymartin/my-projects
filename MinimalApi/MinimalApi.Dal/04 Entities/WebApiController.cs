using Microsoft.EntityFrameworkCore;
using Stratos.Core;

namespace MinimalApi.Dal;

[EntityTypeConfiguration(typeof(WebApiControllerConfiguration))]
public class WebApiController : EntityBase<WebApiController, int>
{
    public override int Id { get; set; }

    public int WebApiId { get; set; }
    public string UriName { get; set; }
}
