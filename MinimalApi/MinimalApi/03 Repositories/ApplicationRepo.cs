using Microsoft.EntityFrameworkCore;
using MinimalApi.Dal;

namespace MinimalApi.Repositories;

public class ApplicationRepo : IApplicationRepo
{
    private readonly MinimalApiDbContext _dbContext;

    public ApplicationRepo(MinimalApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApplicationDto> GetApplicationAsync(int applicationId)
    {
        return await
            (from a in _dbContext.Applications
             where a.Id == applicationId
             select (ApplicationDto)a)
            .FirstOrDefaultAsync();
    }

    public async Task<ApplicationDto> GetApplicationAsync(string applicationName)
    {
        return await
            (from a in _dbContext.Applications
             where a.Name == applicationName
             select (ApplicationDto)a)
            .FirstOrDefaultAsync();
    }

    public async Task<ApplicationDto> GetApplicationAsync(int? applicationId, string applicationName)
    {
        if (applicationId.HasValue)
            return await GetApplicationAsync(applicationId.Value);
        return await GetApplicationAsync(applicationName);
    }

    public async Task<string> GetMinimumVersionAsync(int applicationId)
    {
        return await
            (from a in _dbContext.Applications
             where a.Id == applicationId
             select a.MinimumAssemblyVersion)
            .FirstOrDefaultAsync();
    }
}
