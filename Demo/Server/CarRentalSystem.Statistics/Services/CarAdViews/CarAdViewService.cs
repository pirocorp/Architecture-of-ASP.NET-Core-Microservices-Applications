namespace CarRentalSystem.Statistics.Services.CarAdViews
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Services;
    using Common.Services.Data;
    using Common.Services.Messages;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Models.CarAdViews;

    public class CarAdViewService : DataService<CarAdView>, ICarAdViewService
    {
        public CarAdViewService(StatisticsDbContext db, IPublisher publisher)
            : base(db, publisher)
        {
        }

        public async Task<int> GetTotalViews(int carAdId)
            => await this
                .All()
                .CountAsync(v => v.CarAdId == carAdId);

        public async Task<IEnumerable<CarAdViewOutputModel>> GetTotalViews(
            IEnumerable<int> ids)
            => await this
                .All()
                .Where(v => ids.Contains(v.CarAdId))
                .GroupBy(v => v.CarAdId)
                .Select(gr => new CarAdViewOutputModel
                {
                    CarAdId = gr.Key,
                    TotalViews = gr.Count()
                })
                .ToListAsync();
    }
}
