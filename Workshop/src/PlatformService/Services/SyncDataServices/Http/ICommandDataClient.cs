namespace PlatformService.Services.SyncDataServices.Http
{
    using System.Threading.Tasks;

    using PlatformService.Models;

    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(PlatformRead message);
    }
}
