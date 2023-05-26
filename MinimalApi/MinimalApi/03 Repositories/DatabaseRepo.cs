using Microsoft.EntityFrameworkCore;
using MinimalApi.Dal;

namespace MinimalApi.Repositories;

public class DatabaseRepo : IDatabaseRepo
{
    private readonly MinimalApiDbContext _dbContext;

    public DatabaseRepo(MinimalApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GetCorporateDatabaseNameAsync(EnvironmentTypes environmentType, DatabaseSchemaTypes databaseSchemaType)
    {
        return await
            (from d in _dbContext.Databases
             where d.Type == DatabaseTypes.Corporate
                && d.SchemaType == databaseSchemaType
                && d.EnvironmentType == environmentType
             select d.Name)
            .SingleOrDefaultAsync();
    }

    public async Task<string> GetDatabaseNameAsync(EnvironmentTypes environmentType, int facilityId)
    {
        return await
            (from d in _dbContext.Databases
             where d.Type == DatabaseTypes.Plant
                && (d.SchemaType == DatabaseSchemaTypes.Ipfs
                || d.SchemaType == DatabaseSchemaTypes.Pfs)
                && d.EnvironmentType == environmentType
                && d.Facilities.Any(x => x.Id == facilityId)
             select d.Name)
            .SingleOrDefaultAsync();
    }

    public async Task<string> GetDatabaseNameAsync(string childDatabaseName)
    {
        return await
            (from d in _dbContext.Databases
             where d.Name == childDatabaseName
             select d.Parent.Name)
            .SingleOrDefaultAsync();
    }
}
