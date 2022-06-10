﻿namespace PlatformService
{
    using System;

    using Common.Infrastructure.ConfigurationOptions;
    using Common.Infrastructure.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using PlatformService.Data;
    using PlatformService.Infrastructure.ConfigurationOptions;
    using PlatformService.Infrastructure.Extensions;
    using PlatformService.Services;
    using PlatformService.Services.AsyncDataServices;
    using PlatformService.Services.SyncDataServices.Http;

    public static class Program
    {
        private static string sqlServerConnectionString = string.Empty;

        private static IConfigurationSection? commandServiceOptions;

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
            sqlServerConnectionString = configuration
                .GetConnectionString("PlatformsConnection");

            commandServiceOptions = configuration
                .GetSection(CommandServiceOptions.CommandService);

            rabbitMqOptions = configuration
                .GetSection(RabbitMqOptions.RabbitMq);
        }

        private static void ConfigureServices(IServiceCollection services, IHostEnvironment env)
        {
            services.Configure<CommandServiceOptions>(commandServiceOptions);
            services.Configure<RabbitMqOptions>(rabbitMqOptions);

            if (env.IsProduction())
            {
                services.AddDbContext<PlatformDbContext>(options =>
                {
                    options.UseSqlServer(sqlServerConnectionString);
                });
            }
            else
            {
                services.AddDbContext<PlatformDbContext>(options =>
                {
                    options.UseInMemoryDatabase("In Memory");
                });
            }

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
            services.AddTransient<IPlatformsService, PlatformsService>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();

            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        private static void ConfigureMiddleware(WebApplication app, IServiceProvider services)
        {
            app.UseDatabaseMigrations<PlatformDbContext>();
            app.SeedDatabase();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
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
