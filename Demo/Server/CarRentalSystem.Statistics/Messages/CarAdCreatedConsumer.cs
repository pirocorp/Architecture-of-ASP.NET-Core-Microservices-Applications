namespace CarRentalSystem.Statistics.Messages
{
    using System.Threading.Tasks;
    using Common.Messages.Dealers;
    using MassTransit;
    using Services.Statistics;

    public class CarAdCreatedConsumer : IConsumer<CarAdCreatedMessage>
    {
        private readonly IStatisticsService statistics;

        public CarAdCreatedConsumer(IStatisticsService statistics)
        {
            this.statistics = statistics;
        }

        public async Task Consume(ConsumeContext<CarAdCreatedMessage> context)
        {
            var message = context.Message;

            await this.statistics.AddCarAd();
        }
    }
}
