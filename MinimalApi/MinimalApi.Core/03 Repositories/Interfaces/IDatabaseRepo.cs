namespace MinimalApi.Core;

public interface IDatabaseRepo
{
    Task<string> GetCorporateDatabaseNameAsync(EnvironmentTypes environmentType, DatabaseSchemaTypes databaseSchemaType);
    Task<string> GetDatabaseNameAsync(EnvironmentTypes environmentType, int facilityId);
    Task<string> GetDatabaseNameAsync(string childDatabaseName);
}