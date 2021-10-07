namespace CarRentalSystem.Dealers.Gateway.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Common.Controllers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.CarAds;
    using Services.CarAds;
    using Services.CarAdViews;

    public class CarAdsController : ApiController
    {
        private readonly ICarAdsService carAdsService;

        private readonly ICarAdViewsService carAdViewsService;
        private readonly IMapper mapper;

        public CarAdsController(
            ICarAdsService carAdsService, 
            ICarAdViewsService carAdViewsService,
            IMapper mapper)
        {
            this.carAdsService = carAdsService;
            this.carAdViewsService = carAdViewsService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        [Route(nameof(Mine))]
        public async Task<IEnumerable<MineCarAdOutputModel>> Mine()
        {
            var carAds = await this.carAdsService.Mine();

            var mineCarAds = this.mapper
                .Map<IEnumerable<MineCarAdOutputModel>>(carAds.CarAds)
                .ToDictionary(c => c.Id, c => c);

            var mineCarAdIds = mineCarAds
                .Keys
                .ToList();

            var mineCarAdViews = (await this.carAdViewsService.TotalViews(mineCarAdIds))
                .ToList();

            foreach (var adViews in mineCarAdViews)
            {
                mineCarAds[adViews.CarAdId].TotalViews = adViews.TotalViews;
            }

            return mineCarAds.Values.ToList();
        }
    }
}
