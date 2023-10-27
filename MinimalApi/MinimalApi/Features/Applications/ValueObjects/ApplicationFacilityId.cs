using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.Applications;

public sealed class ApplicationFacilityId : AggregateRootId<int>
{
    public override int Value { get; protected set; }

    private ApplicationFacilityId() { }
    private ApplicationFacilityId(int value)
    {
        Value = value;
    }

    public static ApplicationFacilityId Create(int value) => new(value);
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}