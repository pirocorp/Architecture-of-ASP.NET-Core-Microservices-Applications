namespace PlatformService.Services.AsyncDataServices
{
    using PlatformService.Models;

    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublished model);
    }
}
