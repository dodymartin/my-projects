using MinimalApi.Dom.Common.Models;

namespace MinimalApi.Dom.Facilities.ValueObjects;

public sealed class FacilityId : AggregateRootId<int>
{
    public override int Value { get; protected set; }

    private FacilityId() { }
    private FacilityId(int value)
    {
        Value = value;
    }

    public static FacilityId Create(int value) => new(value);
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
