namespace MinimalApi.Api.Core;

public interface IDatabases
{
    IDatabase this[string databaseName] { get; }
}
