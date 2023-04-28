using Microsoft.EntityFrameworkCore;
using MinimalApi.Entities;

namespace MinimalApi.Version;

public interface IVersionDataContext
{
    DbSet<Application> Applications { get; }
    DbSet<ApplicationFacility> ApplicationFacilities { get; }
}