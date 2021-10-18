namespace CarRentalSystem.Identity
{
    using Common.Infrastructure;
    using Common.Services;
    using Common.Services.Data;
    using Data;
    using Infrastructure;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Services.Identity;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
            => services
                .Configure<IdentitySettings>(
                    this.Configuration.GetSection(nameof(IdentitySettings)),
                    config => config.BindNonPublicProperties = true)
                .AddUserStorage()
                .AddWebService<IdentityDbContext>(
                    this.Configuration, 
                    messagingHealthChecks: false)
                .AddTransient<IDataSeeder, IdentityDataSeeder>()
                .AddTransient<IIdentityService, IdentityService>()
                .AddTransient<ITokenGeneratorService, TokenGeneratorService>();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            => app
                .UseWebService(env)
                .MigrateDatabase()
                .SeedData();
    }
}
