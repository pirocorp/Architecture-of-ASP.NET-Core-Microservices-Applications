namespace CarRentalSystem.Schedule.Services
{
    using System.Linq;
    using System.Threading.Tasks;using Common.Services;
    using Common.Services.Data;
    using Common.Services.Messages;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class RentedCarService : DataService<RentedCar>, IRentedCarService
    {
        public RentedCarService(DbContext db, IPublisher publisher)
            : base(db, publisher)
        {
        }

        public async Task UpdateInformation(int carAdId, string manufacturer, string model)
        {
            var rentedCars = await this
                .All()
                .Where(rc => rc.CarAdId == carAdId)
                .ToListAsync();

            foreach (var rentedCar in rentedCars)
            {
                rentedCar.Information = $"{manufacturer} {model}";
            }

            await this.Data.SaveChangesAsync();
        }
    }
}
