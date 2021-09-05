namespace MovieDatabase.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class MovieDatabaseDbContext : IdentityDbContext<User, Role, string>
    {
        public MovieDatabaseDbContext(DbContextOptions<MovieDatabaseDbContext> options)
            : base(options)
        {
            
        }
    }
}
