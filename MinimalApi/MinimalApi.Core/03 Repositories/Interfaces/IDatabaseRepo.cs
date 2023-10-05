namespace MinimalApi.Core;

public interface IDatabaseRepo
{
    Task<string> GetCorporateDatabaseNameAsync(EnvironmentTypes environmentType, DatabaseSchemaTypes databaseSchemaType);
    Task<string> GetDatabaseNameAsync(EnvironmentTypes environmentType, FacilityId facilityId);
    Task<string> GetDatabaseNameAsync(string childDatabaseName);
}