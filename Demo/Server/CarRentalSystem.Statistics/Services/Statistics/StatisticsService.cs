namespace CarRentalSystem.Statistics.Services.Statistics
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Common.Services.Data;
    using Common.Services.Messages;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Models.Statistics;

    public class StatisticsService : DataService<Statistics>, IStatisticsService
    {
        private readonly IMapper mapper;

        public StatisticsService(StatisticsDbContext db, IPublisher publisher, IMapper mapper)
            : base(db, publisher)
        {
            this.mapper = mapper;
        }

        public async Task<StatisticsOutputModel> Full()
            => await this.mapper
                .ProjectTo<StatisticsOutputModel>(this.All())
                .SingleOrDefaultAsync();
    }
}
