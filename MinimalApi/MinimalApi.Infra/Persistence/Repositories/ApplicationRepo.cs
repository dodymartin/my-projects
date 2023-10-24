using Microsoft.EntityFrameworkCore;
using MinimalApi.App;
using MinimalApi.App.Interfaces;
using MinimalApi.Dom.Applications;
using ApplicationId = MinimalApi.Dom.Applications.ValueObjects.ApplicationId;

namespace MinimalApi.Infra.Persistence.Repositories;

public class ApplicationRepo : IApplicationRepo
{
    private readonly IMinimalApiDbContext _dbContext;

    public ApplicationRepo(IMinimalApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Application?> GetApplicationAsync(int applicationId, CancellationToken cancellationToken)
    {
        return await 
            _dbContext.Applications.FindAsync(ApplicationId.Create(applicationId), cancellationToken);
    }

    public async Task<Application?> GetApplicationAsync(string applicationName, CancellationToken cancellationToken)
    {
        return await
            (from a in _dbContext.Applications
             where a.Name == applicationName
             select a)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<string?> GetMinimumVersionAsync(int applicationId, CancellationToken cancellationToken)
    {
        return await
            (from a in _dbContext.Applications
             where a.Id == ApplicationId.Create(applicationId)
             select a.MinimumAssemblyVersion)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<string?> GetMinimumVersionAsync(int applicationId, int facilityId, CancellationToken cancellationToken)
    {
        var sql = $@"
        select
            af.min_asmbly_ver ""Value""
        from
            cmn_mstr.apln_fac a
        where
            af.apln_id = {applicationId}
        and af.fac_id = {facilityId}
        ";

        return await
            _dbContext.Database
            .SqlQueryRaw<string>(sql)
            .SingleOrDefaultAsync(cancellationToken);
    }
}
