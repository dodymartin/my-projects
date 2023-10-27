using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.Applications;

public sealed class ApplicationVersionId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private ApplicationVersionId() { }
    private ApplicationVersionId(Guid value)
    {
        Value = value;
    }

    public static ApplicationVersionId CreateUnique() => new(Guid.NewGuid());
    public static ApplicationVersionId Create(Guid value) => new(value);
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}