namespace PlatformService.Services
{
    using PlatformService.Models;

    public interface IPlatformsService
    {
        Task<IEnumerable<PlatformRead>> GetAllPlatforms();

        Task<PlatformRead?> GetPlatformById(int id);

        Task<PlatformRead> CreatePlatform(PlatformCreate inputData);
    }
}
