using Microsoft.EntityFrameworkCore;
using MinimalApi.Dal;

namespace MinimalApi.Repositories;

public class ApplicationFacilityRepo : IApplicationFacilityRepo
{
    private readonly MinimalApiDbContext _dbContext;

    public ApplicationFacilityRepo(MinimalApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GetMinimumVersionAsync(int applicationId, int facilityId)
    {
        return await
            (from af in _dbContext.ApplicationFacilities
             where af.Application.Id == applicationId
                && af.Facility.Id == facilityId
             select af.MinimumAssemblyVersion)
            .FirstOrDefaultAsync();
    }
}
