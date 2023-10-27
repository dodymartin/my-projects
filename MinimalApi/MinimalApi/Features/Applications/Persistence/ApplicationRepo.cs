using Microsoft.EntityFrameworkCore;

namespace MinimalApi.Api.Features.Applications;

public class ApplicationRepo : IApplicationRepo
{
    private readonly IApplicationDbContext _dbContext;

    public ApplicationRepo(IApplicationDbContext dbContext)
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
