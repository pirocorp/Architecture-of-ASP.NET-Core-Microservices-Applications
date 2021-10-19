namespace CarRentalSystem.Dealers.Services.Manufacturers
{
    using System.Threading.Tasks;
    using Common.Services.Data;
    using Common.Services.Messages;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class ManufacturerService : DataService<Manufacturer>, IManufacturerService
    {
        public ManufacturerService(DealersDbContext db, IPublisher publisher) 
            : base(db, publisher)
        {
        }

        public async Task<Manufacturer> FindByName(string name)
            => await this
                .All()
                .FirstOrDefaultAsync(m => m.Name == name);
    }
}
