namespace PlatformService.Data
{
    using Microsoft.EntityFrameworkCore;

    using PlatformService.Data.Models;

    public class PlatformDbContext : DbContext
    {
        public PlatformDbContext(DbContextOptions<PlatformDbContext> options)
            : base(options)
        {
        }

        public DbSet<Platform> Platforms => this.Set<Platform>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
