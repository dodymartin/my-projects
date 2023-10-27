using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.Databases;

public interface IDatabaseRepo
{
    Task<string?> GetCorporateDatabaseNameAsync(EnvironmentTypes environmentType, DatabaseSchemaTypes databaseSchemaType, CancellationToken cancellationToken);
    Task<string?> GetDatabaseNameAsync(EnvironmentTypes environmentType, int facilityId, CancellationToken cancellationToken);
    Task<string?> GetParentDatabaseNameAsync(string childDatabaseName, CancellationToken cancellationToken);
}