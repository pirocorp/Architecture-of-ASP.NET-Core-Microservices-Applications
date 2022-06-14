namespace PlatformService.Infrastructure.Extensions
{
    using System.IO;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Hosting;

    public static class WebApplicationExtensions
    {
        public static IEndpointConventionBuilder PlatformsProtoFileEndpoint(this WebApplication app)
            => app.MapGet(
                "/protos/platforms.proto",
                async context =>
                {
                    var path = app.Environment.IsDevelopment()
                        ? "../Common/Protos/platforms.proto"
                        : "./Protos/platforms.proto";

                    var content = await File.ReadAllTextAsync(path);
                    await context.Response.WriteAsync(content);
                });
    }
}
