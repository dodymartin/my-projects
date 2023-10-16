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

    public async Task<Application?> GetApplicationAsync(ApplicationId? applicationId, string applicationName)
    {
        if (applicationId is not null)
            return await GetApplicationAsync(applicationId);
        return await GetApplicationAsync(applicationName);
    }

    public async Task<string?> GetMinimumVersionAsync(ApplicationId applicationId)
    {
        return await
            (from a in _dbContext.Applications
             where a.Id == applicationId
             select a.MinimumAssemblyVersion)
            .FirstOrDefaultAsync();
    }

    #region Private Methods

    private async Task<Application?> GetApplicationAsync(ApplicationId applicationId)
    {
        return await
            (from a in _dbContext.Applications
             where a.Id == applicationId
             select a)
            .FirstOrDefaultAsync();
    }

    private async Task<Application?> GetApplicationAsync(string applicationName)
    {
        return await
            (from a in _dbContext.Applications
             where a.Name == applicationName
             select a)
            .FirstOrDefaultAsync();
    }

    #endregion
}
