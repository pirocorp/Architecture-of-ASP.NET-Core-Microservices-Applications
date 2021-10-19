namespace CarRentalSystem.Admin
{
    using System.Reflection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using CarRentalSystem.Admin.Services.Identity;
    using Common.Infrastructure;
    using Common.Services.Identity;
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using Services.Dealers;
    using Services.Statistics;

    public class Startup
    {
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var serviceEndpoints = this.Configuration
                .GetSection(nameof(ServiceEndpoints))
                .Get<ServiceEndpoints>(config => config.BindNonPublicProperties = true);

            services
                .AddAutoMapperProfile(Assembly.GetExecutingAssembly())
                .AddTokenAuthentication(this.Configuration)
                .AddHealth(
                    this.Configuration,
                    databaseHealthChecks: false,
                    messagingHealthChecks: false)
                .AddScoped<ICurrentTokenService, CurrentTokenService>()
                .AddTransient<JwtCookieAuthenticationMiddleware>()
                .AddExternalService<IIdentityService>(serviceEndpoints.Identity)
                .AddExternalService<IStatisticsService>(serviceEndpoints.Statistics)
                .AddExternalService<IDealersService>(serviceEndpoints.Dealers)
                .AddControllers(options => options
                    .Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app
                    .UseDeveloperExceptionPage();
            }
            else
            {
                app
                    .UseExceptionHandler("/Home/Error")
                    .UseHsts();
            }

            app
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseJwtCookieAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints => endpoints
                    .MapHealthChecks()
                    .MapDefaultControllerRoute());
        }
    }
}
