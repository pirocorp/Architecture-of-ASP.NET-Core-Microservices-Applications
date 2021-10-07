namespace CarRentalSystem.Dealers.Gateway.Models.CarAds
{
    using AutoMapper;
    using Common.Model;

    public class MineCarAdOutputModel : CarAdOutputModel, IMapFrom<CarAdOutputModel>
    {
        public int TotalViews { get; set; }
    }
}
