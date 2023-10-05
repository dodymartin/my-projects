using Microsoft.EntityFrameworkCore;

namespace MinimalApi.Core;

public class ApplicationFacilityRepo : IApplicationFacilityRepo
{
    private readonly IMinimalApiDbContext _dbContext;

    public ApplicationFacilityRepo(IMinimalApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GetMinimumVersionAsync(ApplicationId applicationId, FacilityId facilityId)
    {
        return await
            (from af in _dbContext.ApplicationFacilities
             where af.Application.Id == applicationId
                && af.Facility.Id == facilityId
             select af.MinimumAssemblyVersion)
            .FirstOrDefaultAsync();
    }
}
