﻿using System.Text;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Shared;

namespace MinimalApi.Core;

public class WebApiVersionRepo : IWebApiVersionRepo
{
    private readonly IMinimalApiDbContext _dbContext;

    public WebApiVersionRepo(IMinimalApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GetBaseUriAsync(BaseUriRequest request)
    {
        return await
            (from wav in _dbContext.WebApiVersions
                where wav.WebApi.ApplicationId == new ApplicationId(request.ApplicationId.Value)
                && request.ApplicationVersion.StartsWith(wav.Version.Substring(0, request.ApplicationVersion.Length))
                select $@"http{(wav.WebApi.UseHttps ? "s" : "")}://+:{wav.Port}")
            .FirstOrDefaultAsync();
    }
}
