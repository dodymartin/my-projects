using Microsoft.EntityFrameworkCore;
using V_Slice.Features.Todo;

namespace V_Slice.Common
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TodoEntity> Todos { get; set; } = null!;
    }
}