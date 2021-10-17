namespace CarRentalSystem.Dealers.Data
{
    using System.Reflection;
    using Common.Data;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class DealersDbContext : MessageDbContext
    {
        public DealersDbContext(DbContextOptions<DealersDbContext> options)
            : base(options)
        {
            this.ConfigurationsAssembly = Assembly.GetExecutingAssembly();
        }

        public DbSet<CarAd> CarAds { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<Dealer> Dealers { get; set; }

        protected override Assembly ConfigurationsAssembly { get; }
    }
}