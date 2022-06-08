namespace PlatformService.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PlatformService.Models;

    public interface IPlatformsService
    {
        Task<IEnumerable<PlatformRead>> GetAllPlatforms();

        Task<PlatformRead?> GetPlatformById(int id);

        Task<PlatformRead> CreatePlatform(PlatformCreate inputData);
    }
}
