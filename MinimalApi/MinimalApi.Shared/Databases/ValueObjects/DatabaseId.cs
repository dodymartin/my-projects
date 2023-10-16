using MinimalApi.Dom.Common.Models;

namespace MinimalApi.Dom.Databases.ValueObjects;

public sealed class DatabaseId : AggregateRootId<string>
{
    public override string Value { get; protected set; } = string.Empty;

    private DatabaseId() { }
    private DatabaseId(string value)
    {
        Value = value;
    }

    public static DatabaseId Create(string value) => new(value);
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
