namespace CommandService.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CommandService.Models;

    public interface IPlatformsService
    {
        Task<bool> Exists(int platformId);

        Task<IEnumerable<PlatformRead>> GetAll();
    }
}
