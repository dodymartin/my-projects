using Microsoft.EntityFrameworkCore;

namespace V_Slice.Common
{
    public static class DependencyInjection
    {
        public static void AddEfCore(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });
        }
    }
}