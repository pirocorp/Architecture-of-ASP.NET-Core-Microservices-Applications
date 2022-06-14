namespace CommandService.Infrastructure.Extensions
{
    using CommandService.Data;
    using CommandService.Services;
    using CommandService.Services.SyncDataServices.Grpc;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SyncData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<CommandDbContext>();

            var platformsService = scope.ServiceProvider.GetRequiredService<IPlatformsService>();
            var grpcClient = scope.ServiceProvider.GetRequiredService<IPlatformDataClient>();
            var platforms = grpcClient
                .GetAllPlatforms()
                .GetAwaiter()
                .GetResult();

            foreach (var platform in platforms)
            {
                if (!platformsService.ExternalExists(platform.ExternalId).GetAwaiter().GetResult())
                {
                    dbContext.Add(platform);
                }
            }

            dbContext.SaveChanges();

            return app;
        }
    }
}
