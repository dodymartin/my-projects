using Microsoft.Extensions.Options;
using MinimalApi.App.Interfaces;
using MinimalApi.Dom.Enumerations;
using MinimalApi.Dom.Facilities.ValueObjects;
using MinimalApi.Shared;

namespace MinimalApi.App;

public class DatabaseService
{
    private readonly AppSettings _appSettings;
    private readonly IDatabaseRepo _databaseRepo;

    public DatabaseService(IOptions<AppSettings> appSettings, IDatabaseRepo databaseRepo)
    {
        _appSettings = appSettings.Value;
        _databaseRepo = databaseRepo;
    }

    public async Task<string> GetCorporateDatabaseNameAsync(DatabaseSchemaTypes databaseSchemaType)
    {
        var environmentType = (EnvironmentTypes)Enum.Parse(typeof(EnvironmentTypes), _appSettings.EnvironmentType);
        return await _databaseRepo.GetCorporateDatabaseNameAsync(environmentType, databaseSchemaType);
    }

    public async Task<string> GetDatabaseNameAsync(DatabaseRequest request)
    {
        var environmentType = (EnvironmentTypes)Enum.Parse(typeof(EnvironmentTypes), _appSettings.EnvironmentType);
        return await _databaseRepo.GetDatabaseNameAsync(environmentType, FacilityId.Create(request.FacilityId.Value));
    }

    public async Task<string> GetParentDatabaseNameAsync(DatabaseRequest request)
    {
        return await _databaseRepo.GetDatabaseNameAsync(request.ChildDatabaseName);
    }
}
