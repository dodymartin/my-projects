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
            (from a in _dbContext.Applications
             where a.Id == ApplicationId.Create(applicationId)
             select a)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<Application?> GetApplicationAsync(string applicationName, CancellationToken cancellationToken)
    {
        return await
            (from a in _dbContext.Applications
             where a.Name == applicationName
             select a)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<string?> GetMinimumVersionAsync(int applicationId, int facilityId, CancellationToken cancellationToken)
    {
        FormattableString sql = $"""
        select
            af.min_asmbly_ver "Value"
        from
            cmn_mstr.apln_fac af
        where
            af.apln_id = {applicationId}
        and af.fac_id = {facilityId}
        """;

        return await
            _dbContext.Database
            .SqlQuery<string>(sql)
            .SingleOrDefaultAsync(cancellationToken);
    }
}
