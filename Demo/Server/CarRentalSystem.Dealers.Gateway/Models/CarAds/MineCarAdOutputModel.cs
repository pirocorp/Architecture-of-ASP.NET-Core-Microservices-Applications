namespace CarRentalSystem.Dealers.Gateway.Models.CarAds
{
    using Common.Model;

    public class MineCarAdOutputModel : CarAdOutputModel, IMapFrom<CarAdOutputModel>
    {
        public int TotalViews { get; set; }
    }
}
