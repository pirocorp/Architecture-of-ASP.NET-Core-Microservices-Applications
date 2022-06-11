namespace CommandService
{
    using System;

    using CommandService.Data;
    using CommandService.Services;
    using Common.Infrastructure.ConfigurationOptions;
    using Common.Infrastructure.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using static Common.Infrastructure.ApiConstants;

    public static class Program
    {
        private static string sqlServerConnectionString = string.Empty;

        private static IConfigurationSection? rabbitMqOptions;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureConfiguration(builder.Configuration);
            ConfigureServices(builder.Services, builder.Environment);

            var app = builder.Build();

            ConfigureMiddleware(app, app.Services);
            ConfigureEndpoints(app, app.Services);

            app.Run();
        }

        private static void ConfigureConfiguration(IConfiguration configuration)
        {
            sqlServerConnectionString = configuration.GetConnectionString("CommandsConnection");

            rabbitMqOptions = configuration.GetSection(RabbitMqOptions.RabbitMq);
        }

        private static void ConfigureServices(IServiceCollection services, IHostEnvironment env)
        {
            services.Configure<RabbitMqOptions>(rabbitMqOptions);

            if (env.IsProduction())
            {
                services.AddDbContext<CommandDbContext>(options =>
                {
                    options.UseSqlServer(sqlServerConnectionString);
                });
            }
            else
            {
                services.AddDbContext<CommandDbContext>(options =>
                {
                    options.UseInMemoryDatabase("In Memory");
                });
            }

            services.AddControllers();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<ICommandsService, CommandsService>();
            services.AddTransient<IPlatformsService, PlatformsService>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        private static void ConfigureMiddleware(WebApplication app, IServiceProvider services)
        {
            app.UseDatabaseMigrations<CommandDbContext>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseExceptionHandler(DevelopmentErrorRoute);
            }
            else
            {
                app.UseExceptionHandler(ProductionErrorRoute);
            }

            // app.UseHttpsRedirection();
            app.UseAuthorization();
        }

        private static void ConfigureEndpoints(IEndpointRouteBuilder app, IServiceProvider services)
        {
            app.MapControllers();
        }
    }
}
