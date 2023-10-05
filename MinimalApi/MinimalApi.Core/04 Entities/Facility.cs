using Stratos.Core;

namespace MinimalApi.Core;

public record FacilityId(int Value);

public class Facility : EntityBase<Facility, FacilityId>
{
    public override FacilityId Id { get; set; }

    public string Name { get; set; }

    public List<Database> Databases { get; } = new();
}
