namespace PlatformService.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    using PlatformService.Data;
    using PlatformService.Data.Models;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;

            using var dbContext = serviceProvider.GetRequiredService<PlatformDbContext>();

            SeedPlatforms(dbContext);

            return app;
        }

        private static void SeedPlatforms(PlatformDbContext dbContext)
        {
            if (dbContext.Platforms.Any())
            {
                return;
            }

            var platforms = new List<Platform>()
            {
                new () { Name = ".NET", Publisher = "Microsoft", Cost = "Free" },
                new () { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                new () { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" },
            };

            dbContext.Platforms.AddRange(platforms);
            dbContext.SaveChanges();
        }
    }
}
