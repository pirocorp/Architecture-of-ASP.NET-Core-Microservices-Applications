namespace CommandService.Data
{
    using CommandService.Data.Models;

    using Microsoft.EntityFrameworkCore;

    public class CommandDbContext : DbContext
    {
        public CommandDbContext(DbContextOptions<CommandDbContext> options)
            : base(options)
        {
        }

        public DbSet<Command> Commands => this.Set<Command>();

        public DbSet<Platform> Platforms => this.Set<Platform>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
