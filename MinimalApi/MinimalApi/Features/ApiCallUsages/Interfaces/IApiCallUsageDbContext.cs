using Microsoft.EntityFrameworkCore;

namespace MinimalApi.Api.Features.ApiCallUsages;

public interface IApiCallUsageDbContext
{
    Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade Database { get; }
    DbSet<ApiCallUsage> ApiCallUsages { get; }
}