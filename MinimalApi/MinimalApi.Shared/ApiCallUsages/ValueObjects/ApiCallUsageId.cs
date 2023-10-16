using MinimalApi.Dom.Common.Models;

namespace MinimalApi.Dom.ApiCallUsages.ValueObjects;

public sealed class ApiCallUsageId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private ApiCallUsageId() { }
    private ApiCallUsageId(Guid value)
    {
        Value = value;
    }

    public static ApiCallUsageId CreateUnique() => new(Guid.NewGuid());
    public static ApiCallUsageId Create(Guid value) => new(value);
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}