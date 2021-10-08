namespace CarRentalSystem.Schedule.Messages
{
    using System.Threading.Tasks;
    using Common.Messages.Dealers;
    using Data;
    using MassTransit;
    using Services;

    public class CarAdUpdatedConsumer : IConsumer<CarAdUpdatedMessage>
    {
        private readonly IRentedCarService rentedCarService;

        public CarAdUpdatedConsumer(IRentedCarService rentedCarService)
        {
            this.rentedCarService = rentedCarService;
        }

        public async Task Consume(ConsumeContext<CarAdUpdatedMessage> context)
            => await this.rentedCarService.UpdateInformation(
                    context.Message.CarAdId,
                    context.Message.Manufacturer,
                    context.Message.Model);
    }
}
