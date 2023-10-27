using Microsoft.EntityFrameworkCore;
using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.Applications;

public interface IApplicationDbContext
{
    Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade Database { get; }
    DbSet<Application> Applications { get; }
    IDbContextSettings Settings { get; }
}