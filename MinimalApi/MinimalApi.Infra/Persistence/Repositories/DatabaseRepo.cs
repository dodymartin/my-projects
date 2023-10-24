using Microsoft.EntityFrameworkCore;
using MinimalApi.App;
using MinimalApi.App.Interfaces;
using MinimalApi.Dom.Enumerations;

namespace MinimalApi.Infra.Persistence.Repositories;

public class DatabaseRepo : IDatabaseRepo
{
    private readonly IMinimalApiDbContext _dbContext;

    public DatabaseRepo(IMinimalApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string?> GetCorporateDatabaseNameAsync(EnvironmentTypes environmentType, DatabaseSchemaTypes databaseSchemaType, CancellationToken cancellationToken)
        => await
            (from d in _dbContext.Databases
             where d.Type == DatabaseTypes.Corporate
             && d.SchemaType == databaseSchemaType
             && d.EnvironmentType == environmentType
             select d.Name)
            .SingleOrDefaultAsync(cancellationToken);

    public async Task<string?> GetDatabaseNameAsync(EnvironmentTypes environmentType, int facilityId, CancellationToken cancellationToken)
        => await
            (from d in _dbContext.Databases
             where d.Type == DatabaseTypes.Plant
                && (d.SchemaType == DatabaseSchemaTypes.Ipfs
                || d.SchemaType == DatabaseSchemaTypes.Pfs)
                && d.EnvironmentType == environmentType
                && d.FacilityIds.Any(x => x.Value == facilityId)
             select d.Name)
            .SingleOrDefaultAsync(cancellationToken);

    public async Task<string?> GetParentDatabaseNameAsync(string childDatabaseName, CancellationToken cancellationToken)
    {
        FormattableString sql = $@"
        select
            pd.nm ""Value""
        from
            cmn_mstr.db d
            join cmn_mstr.db pd on d.prnt_db_id = pd.db_id
        where
            d.nm = {childDatabaseName}
        ";

        return await
            _dbContext.Database
            .SqlQuery<string>(sql)
            .SingleOrDefaultAsync(cancellationToken);
    }
}
