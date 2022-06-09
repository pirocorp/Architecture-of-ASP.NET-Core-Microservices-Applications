namespace CommandService.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CommandService.Models;

    public interface ICommandsService
    {
        Task<IEnumerable<CommandRead>> GetAll(int platformId);

        Task<CommandRead?> GetCommandForPlatform(int platformId, int commandId);

        Task<CommandRead> CreateCommandForPlatform(int platformId, CommandCreate model);
    }
}
