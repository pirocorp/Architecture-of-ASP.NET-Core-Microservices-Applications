namespace CommandService.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using CommandService.Data;
    using CommandService.Data.Models;
    using CommandService.Models;

    using Microsoft.EntityFrameworkCore;

    public class CommandsService : ICommandsService
    {
        private readonly CommandDbContext context;
        private readonly IMapper mapper;

        public CommandsService(
            CommandDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CommandRead>> GetAll(int platformId)
            => await this.context.Commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.Platform.Name)
                .ThenBy(c => c.HowTo)
                .Select(c => this.mapper.Map<CommandRead>(c))
                .ToListAsync();

        public async Task<CommandRead?> GetCommandForPlatform(int platformId, int commandId)
            => await this.context.Commands
                .Where(c => c.PlatformId == platformId && c.Id == commandId)
                .Select(c => this.mapper.Map<CommandRead>(c))
                .FirstOrDefaultAsync();

        public async Task<CommandRead> CreateCommandForPlatform(int platformId, CommandCreate model)
        {
            var command = this.mapper.Map<Command>(model);
            command.PlatformId = platformId;

            await this.context.AddAsync(command);
            await this.context.SaveChangesAsync();

            return this.mapper.Map<CommandRead>(command);
        }
    }
}
