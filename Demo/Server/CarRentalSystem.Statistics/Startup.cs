namespace CarRentalSystem.Statistics
{
    using Common.Infrastructure;
    using Common.Services;
    using Data;
    using MassTransit;
    using Messages;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Services.CarAdViews;
    using Services.Statistics;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
            => services
                .AddWebService<StatisticsDbContext>(this.Configuration)
                .AddTransient<IDataSeeder, StatisticsDataSeeder>()
                .AddTransient<IStatisticsService, StatisticsService>()
                .AddTransient<ICarAdViewService, CarAdViewService>()
                .AddMassTransit(mt =>
                {
                    mt.AddConsumer<CarAdCreatedConsumer>();

                    mt.AddBus(bus => Bus.Factory.CreateUsingRabbitMq(rmq =>
                    {
                        rmq.Host("localhost");

                        rmq.ReceiveEndpoint(nameof(CarAdCreatedConsumer), endpoint =>
                        {
                            endpoint.ConfigureConsumer<CarAdCreatedConsumer>(bus);
                        });
                    }));
                })
                .AddMassTransitHostedService();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            => app
                .UseWebService(env)
                .MigrateDatabase()
                .SeedData();
    }
}
