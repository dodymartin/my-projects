using Microsoft.EntityFrameworkCore;
using SSW_Clean.Domain.TodoItems;

namespace SSW_Clean.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    DbSet<TodoItem> TodoItems { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}