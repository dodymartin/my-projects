using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.Applications;

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