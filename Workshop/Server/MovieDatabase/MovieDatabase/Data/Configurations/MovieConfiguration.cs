namespace MovieDatabase.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    using static DataConstants.Movie;

    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(m => m.Id);

            builder
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            builder
                .Property(m => m.MPAA)
                .HasMaxLength(MpaaMaxLength);

            builder
                .Property(m => m.Storyline)
                .IsRequired()
                .HasMaxLength(StorylineMaxLength);

            builder
                .Property(m => m.Poster)
                .IsRequired()
                .HasMaxLength(PosterMaxLength);

            builder
                .Property(m => m.IsAnimation)
                .IsRequired();

            builder
                .HasOne(m => m.Writer)
                .WithMany(w => w.WrittenMovies)
                .HasForeignKey(m => m.WriterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(m => m.Director)
                .WithMany(w => w.DirectedMovies)
                .HasForeignKey(m => m.DirectorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
