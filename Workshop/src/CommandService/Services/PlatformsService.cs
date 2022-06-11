namespace CommandService.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using CommandService.Data;
    using CommandService.Models;

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
    }
}
