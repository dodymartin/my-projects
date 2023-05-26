using Microsoft.Extensions.Options;
using MinimalApi.Dal;
using MinimalApi.Endpoints;

namespace MinimalApi.Services
{
    public class DatabaseService
    {
        private readonly UnitOfWorkService _uow;
        private readonly AppSettings _appSettings;

        public DatabaseService(UnitOfWorkService uow, IOptions<AppSettings> appSettings)
        {
            _uow = uow;
            _appSettings = appSettings.Value;
        }

        public async Task<string> GetCorporateDatabaseNameAsync(DatabaseSchemaTypes databaseSchemaType)
        {
            var environmentType = (EnvironmentTypes)Enum.Parse(typeof(EnvironmentTypes), _appSettings.EnvironmentType);
            return await _uow.DatabaseRepo.GetCorporateDatabaseNameAsync(environmentType, databaseSchemaType);
        }

        public async Task<string> GetDatabaseNameAsync(DatabaseRequest request)
        {
            var environmentType = (EnvironmentTypes)Enum.Parse(typeof(EnvironmentTypes), _appSettings.EnvironmentType);
            return await _uow.DatabaseRepo.GetDatabaseNameAsync(environmentType, request.FacilityId.Value);
        }

        public async Task<string> GetParentDatabaseNameAsync(DatabaseRequest request)
        {
            return await _uow.DatabaseRepo.GetDatabaseNameAsync(request.ChildDatabaseName);
        }
    }
}
