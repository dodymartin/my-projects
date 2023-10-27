using Microsoft.EntityFrameworkCore;
using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.Databases;

public interface IDatabaseDbContext
{
    Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade Database { get; }
    DbSet<Database> Databases { get; }
    IDbContextSettings Settings { get; }
}