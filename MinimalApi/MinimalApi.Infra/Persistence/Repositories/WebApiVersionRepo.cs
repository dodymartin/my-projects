using MinimalApi.App;
using MinimalApi.App.Interfaces;
using MinimalApi.Shared;

namespace MinimalApi.Infra.Persistence.Repositories;

public class WebApiVersionRepo : IWebApiVersionRepo
{
    private readonly IMinimalApiDbContext _dbContext;

    public WebApiVersionRepo(IMinimalApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string?> GetBaseUriAsync(BaseUriRequest request)
    {
        if (request.ApplicationId is not null && request.ApplicationVersion is not null)
        {
            //return await
            //    (from wav in _dbContext.WebApiVersions
            //     where wav.WebApi.ApplicationId == ApplicationId.Create(request.ApplicationId.Value)
            // && request.ApplicationVersion.StartsWith(wav.Version.Substring(0, request.ApplicationVersion.Length))
            //     select $@"http{(wav.WebApi.UseHttps ? "s" : "")}://+:{wav.Port}")
            //    .FirstOrDefaultAsync();
        }
        return default;
    }
}
