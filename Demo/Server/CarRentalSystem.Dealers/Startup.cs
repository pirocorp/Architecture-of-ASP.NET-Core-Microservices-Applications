namespace CarRentalSystem.Dealers
{
    using System.Reflection;
    using Common.Infrastructure;
    using Common.Services;
    using Data;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Services.CarAds;
    using Services.Categories;
    using Services.Dealers;
    using Services.Manufacturers;

    public class Startup
    {
        public Startup(IConfiguration configuration) 
            => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
            => services
                .AddWebService<DealersDbContext>(this.Configuration, "CarRentalSystem.Dealers", "v1")
                .AddTransient<IDataSeeder, DealersDataSeeder>()
                .AddTransient<IDealerService, DealerService>()
                .AddTransient<ICategoryService, CategoryService>()
                .AddTransient<ICarAdService, CarAdService>()
                .AddTransient<IManufacturerService, ManufacturerService>();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            => app
                .UseWebService(env, "CarRentalSystem.Dealers")
                .MigrateDatabase()
                .SeedData();
    }
}
