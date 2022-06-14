namespace CommandService.Services.SyncDataServices.Grpc
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CommandService.Data.Models;

    public interface IPlatformDataClient
    {
        Task<IEnumerable<Platform>> GetAllPlatforms();
    }
}
