using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.WebApis;

public sealed class WebApiControllerId : AggregateRootId<int>
{
    public override int Value { get; protected set; }

    private WebApiControllerId() { }
    private WebApiControllerId(int value)
    {
        Value = value;
    }

    public static WebApiControllerId Create(int value) => new(value);
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}