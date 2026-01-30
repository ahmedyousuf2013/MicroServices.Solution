using Microsoft.EntityFrameworkCore;
using Stock.Service.Persistence.Context;

namespace Stock.Service.API
{
    public static class BuilderExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using StockServiceContext dbContext =
                scope.ServiceProvider.GetRequiredService<StockServiceContext>();

            dbContext.Database.Migrate();
        }
    }
}
