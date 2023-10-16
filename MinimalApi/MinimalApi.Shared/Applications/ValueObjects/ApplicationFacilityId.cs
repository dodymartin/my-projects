using MinimalApi.Dom.Common.Models;

namespace MinimalApi.Dom.Applications.ValueObjects;

public sealed class ApplicationFacilityId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private ApplicationFacilityId() { }
    private ApplicationFacilityId(Guid value)
    {
        Value = value;
    }

    public static ApplicationFacilityId CreateUnique() => new(Guid.NewGuid());
    public static ApplicationFacilityId Create(Guid value) => new(value);
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}