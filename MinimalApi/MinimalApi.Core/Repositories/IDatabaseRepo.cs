using MinimalApi.Dom.Enumerations;

namespace MinimalApi.App.Interfaces;

public interface IDatabaseRepo
{
    Task<string?> GetCorporateDatabaseNameAsync(EnvironmentTypes environmentType, DatabaseSchemaTypes databaseSchemaType, CancellationToken cancellationToken);
    Task<string?> GetDatabaseNameAsync(EnvironmentTypes environmentType, int facilityId, CancellationToken cancellationToken);
    Task<string?> GetParentDatabaseNameAsync(string childDatabaseName, CancellationToken cancellationToken);
}