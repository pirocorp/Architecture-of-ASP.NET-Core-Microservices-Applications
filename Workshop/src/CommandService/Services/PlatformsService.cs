namespace CommandService.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using CommandService.Data;
    using CommandService.Data.Models;
    using CommandService.Models;

    using Common.Messages;

    using Microsoft.EntityFrameworkCore;

    public class PlatformsService : IPlatformsService
    {
        private readonly CommandDbContext context;
        private readonly IMapper mapper;

        public PlatformsService(
            CommandDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<bool> Exists(int platformId)
            => await this.context.Platforms.AnyAsync(p => p.Id == platformId);

        public async Task<bool> ExternalExists(int externalId)
            => await this.context.Platforms.AnyAsync(p => p.ExternalId == externalId);

        public async Task<IEnumerable<PlatformRead>> GetAll()
            => await this.context.Platforms
                .Select(p => this.mapper.Map<PlatformRead>(p))
                .ToListAsync();

        public async Task<PlatformRead> CreatePlatform(PlatformPublished model)
            => await this.CreatePlatform(this.mapper.Map<Platform>(model));

        public async Task<PlatformRead> CreatePlatform(Platform platform)
        {
            await this.context.Platforms.AddAsync(platform);
            await this.context.SaveChangesAsync();

            return this.mapper.Map<PlatformRead>(platform);
        }
    }
}
