namespace MovieDatabase.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    using static DataConstants.Series;

    public class SeriesConfiguration : IEntityTypeConfiguration<Series>
    {
        public void Configure(EntityTypeBuilder<Series> builder)
        {
            builder.HasKey(s => s.Id);

            builder
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            builder
                .Property(s => s.StartYear)
                .IsRequired();

            builder
                .Property(s => s.MPAA)
                .HasMaxLength(MpaaMaxLength);

            builder
                .Property(s => s.Storyline)
                .IsRequired()
                .HasMaxLength(StorylineMaxLength);

            builder
                .Property(s => s.Poster)
                .IsRequired()
                .HasMaxLength(PosterMaxLength);

            builder
                .Property(s => s.IsAnimation)
                .IsRequired();
        }
    }
}
