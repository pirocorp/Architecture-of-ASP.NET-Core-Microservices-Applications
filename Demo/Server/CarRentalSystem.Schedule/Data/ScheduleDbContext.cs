namespace CarRentalSystem.Schedule.Data
{
    using System.Reflection;
    using Common.Data;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ScheduleDbContext : MessageDbContext
    {
        public ScheduleDbContext(DbContextOptions<ScheduleDbContext> options)
            : base(options)
        {
        }

        public DbSet<Driver> Drivers { get; set; }

        public DbSet<Feedback> Feedback { get; set; }

        public DbSet<RentedCar> RentedCars { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        protected override Assembly ConfigurationsAssembly => Assembly.GetExecutingAssembly();
    }
}
