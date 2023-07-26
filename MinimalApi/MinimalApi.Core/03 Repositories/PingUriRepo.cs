using Microsoft.EntityFrameworkCore;

namespace MinimalApi.Core;

public class PingUriRepo : IPingUriRepo
{
    private readonly IMinimalApiDbContext _dbContext;

    public PingUriRepo(IMinimalApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<string>> GetPingUris(string applicationName, string applicationVersion)
    {
        return await
            (from a in _dbContext.Applications
             join wa in _dbContext.WebApis on a.Id equals wa.ApplicationId
             join wac in _dbContext.WebApiControllers on wa.Id equals wac.WebApiId
             join wav in _dbContext.WebApiVersions on wa.Id equals wav.WebApiId
             where a.Name == applicationName
                && a.FromDirectoryName.ToLower().EndsWith("publish")
                && !wa.UseHttps
                && wav.Version.StartsWith(applicationVersion)
                && !a.Versions.Any(v => v.Version == applicationVersion && !v.FromDirectoryName.ToLower().EndsWith("publish"))
             select
                $"http{(wa.UseHttps ? "s" : "")}://localhost:{wav.Port}/api/{wac.UriName}/ping")
            .ToListAsync();
    }
}
