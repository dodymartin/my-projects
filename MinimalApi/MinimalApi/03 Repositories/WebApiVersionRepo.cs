using Microsoft.EntityFrameworkCore;
using MinimalApi.Dal;
using MinimalApi.Endpoints;

namespace MinimalApi.Repositories;

public class WebApiVersionRepo : IWebApiVersionRepo
{
    private readonly MinimalApiDbContext _dbContext;

    public WebApiVersionRepo(MinimalApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GetBaseUriAsync(BaseUriRequest request)
    {
        return await
            (from wav in _dbContext.WebApiVersions
                where wav.WebApi.ApplicationId == request.ApplicationId.Value
                && request.ApplicationVersion.StartsWith(wav.Version.Substring(0, request.ApplicationVersion.Length))
                select $@"http{(wav.WebApi.UseHttps ? "s" : "")}://+:{wav.Port}")
            .FirstOrDefaultAsync();
    }
}
