using MinimalApi.Dom.Common.Models;

namespace MinimalApi.Dom.Applications.ValueObjects;

public sealed class ApplicationId : AggregateRootId<int>
{
    public override int Value { get; protected set; }

    private ApplicationId() { }
    private ApplicationId(int value)
    {
        Value = value;
    }

    public static ApplicationId Create(int value) => new(value);
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}