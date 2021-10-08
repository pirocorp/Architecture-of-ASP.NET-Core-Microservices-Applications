namespace CarRentalSystem.Schedule
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Common.Infrastructure;
    using Data;
    using MassTransit;
    using Messages;
    using Services;

    public class Startup
    {
        public Startup(IConfiguration configuration)
            => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
            => services
                .AddWebService<ScheduleDbContext>(this.Configuration)
                .AddTransient<IRentedCarService, RentedCarService>()
                .AddMassTransit(mt =>
                {
                    mt.AddConsumer<CarAdUpdatedConsumer>();

                    mt.AddBus(bus => Bus.Factory.CreateUsingRabbitMq(rmq =>
                    {
                        rmq.Host("localhost");

                        rmq.ReceiveEndpoint(nameof(CarAdUpdatedConsumer), endpoint =>
                        {
                            endpoint.ConfigureConsumer<CarAdUpdatedConsumer>(bus);
                        });
                    }));
                })
                .AddMassTransitHostedService();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            => app
                .UseWebService(env)
                .MigrateDatabase();
    }
}
