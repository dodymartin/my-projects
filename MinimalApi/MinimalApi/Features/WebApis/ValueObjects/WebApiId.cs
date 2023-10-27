using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.WebApis;

public sealed class WebApiId : AggregateRootId<int>
{
    public override int Value { get; protected set; }

    private WebApiId() { }
    private WebApiId(int value)
    {
        Value = value;
    }

    public static WebApiId Create(int value) => new(value);
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
