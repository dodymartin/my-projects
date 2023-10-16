using MinimalApi.Dom.Enumerations;
using MinimalApi.Dom.Facilities.ValueObjects;

namespace MinimalApi.App.Interfaces;

public interface IDatabaseRepo
{
    Task<string?> GetCorporateDatabaseNameAsync(EnvironmentTypes environmentType, DatabaseSchemaTypes databaseSchemaType);
    Task<string?> GetDatabaseNameAsync(EnvironmentTypes environmentType, FacilityId facilityId);
    Task<string?> GetDatabaseNameAsync(string childDatabaseName);
}