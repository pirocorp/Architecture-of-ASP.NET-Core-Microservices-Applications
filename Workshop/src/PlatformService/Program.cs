namespace PlatformService
{
    using Microsoft.EntityFrameworkCore;

    using PlatformService.Data;
    using PlatformService.Infrastructure.Extensions;
    using PlatformService.Services;

    public static class Program
    {
        private static string sqlServerConnectionString = string.Empty;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureConfiguration(builder.Configuration);
            ConfigureServices(builder.Services);

            var app = builder.Build();

            ConfigureMiddleware(app, app.Services);
            ConfigureEndpoints(app, app.Services);

            app.Run();
        }

        private static void ConfigureConfiguration(IConfiguration configuration)
        {
            sqlServerConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PlatformDbContext>(options =>
            {
                options.UseSqlServer(sqlServerConnectionString);
            });

            services.AddControllers();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<IPlatformsService, PlatformsService>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        private static void ConfigureMiddleware(WebApplication app, IServiceProvider services)
        {
            app.UseDatabaseMigrations();
            app.SeedDatabase();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
        }

        private static void ConfigureEndpoints(IEndpointRouteBuilder app, IServiceProvider services)
        {
            app.MapControllers();
        }
    }
}
