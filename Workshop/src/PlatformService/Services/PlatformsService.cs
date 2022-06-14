namespace PlatformService.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.EntityFrameworkCore;

    using PlatformService.Data;
    using PlatformService.Data.Models;
    using PlatformService.Models;

    public class PlatformsService : IPlatformsService
    {
        private readonly PlatformDbContext dbContext;
        private readonly IMapper mapper;

        public PlatformsService(
            PlatformDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<T>> GetAllPlatforms<T>()
            => await this.dbContext.Platforms
                .Select(p => this.mapper.Map<T>(p))
                .ToListAsync();

        public async Task<PlatformRead?> GetPlatformById(int id)
            => await this.dbContext.Platforms
                .Where(p => p.Id == id)
                .Select(p => this.mapper.Map<PlatformRead>(p))
                .FirstOrDefaultAsync();

        public async Task<PlatformRead> CreatePlatform(PlatformCreate inputData)
        {
            var model = this.mapper.Map<Platform>(inputData);

            await this.dbContext.AddAsync(model);
            await this.dbContext.SaveChangesAsync();

            return this.mapper.Map<PlatformRead>(model);
        }
    }
}
