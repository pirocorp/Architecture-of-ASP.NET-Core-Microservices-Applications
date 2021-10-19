namespace CarRentalSystem.Schedule.Messages
{
    using System.Threading.Tasks;
    using Common.Data.Models;
    using Common.Messages.Dealers;
    using Common.Services.Messages;
    using Data;
    using MassTransit;
    using Services;

    // TODO: Extract common base class for consumer with method for SavePublishedMessage and IsDuplicated
    public class CarAdUpdatedConsumer : IConsumer<CarAdUpdatedMessage>
    {
        private readonly ScheduleDbContext data;
        private readonly IMessageService messages;
        private readonly IRentedCarService rentedCarService;

        public CarAdUpdatedConsumer(
            ScheduleDbContext data,
            IMessageService messages,
            IRentedCarService rentedCarService)
        {
            this.data = data;
            this.messages = messages;
            this.rentedCarService = rentedCarService;
        }

        public async Task Consume(ConsumeContext<CarAdUpdatedMessage> context)
        {
            var message = context.Message;

            var isDuplicated = await this.messages.IsDuplicated(
                message,
                nameof(CarAdCreatedMessage.CarAdId),
                message.CarAdId);

            if (isDuplicated)
            {
                return;
            }
            
            await this.rentedCarService.UpdateInformation(
                context.Message.CarAdId,
                context.Message.Manufacturer,
                context.Message.Model);

            var dataMessage = new Message(message);

            dataMessage.MarkAsPublished();

            this.data.Messages.Add(dataMessage);

            await this.data.SaveChangesAsync();
        }
    }
}
