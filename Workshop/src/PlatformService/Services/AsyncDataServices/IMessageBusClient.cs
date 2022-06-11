namespace PlatformService.Services.AsyncDataServices
{
    using Common.Messages;

    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublished model);
    }
}
