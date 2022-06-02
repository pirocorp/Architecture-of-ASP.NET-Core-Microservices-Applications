namespace PlatformService.Infrastructure.Extensions
{
    using Microsoft.EntityFrameworkCore;

    using PlatformService.Data;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigrations(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<PlatformDbContext>();
            dbContext.Database.Migrate();

            return app;
        }
    }
}
