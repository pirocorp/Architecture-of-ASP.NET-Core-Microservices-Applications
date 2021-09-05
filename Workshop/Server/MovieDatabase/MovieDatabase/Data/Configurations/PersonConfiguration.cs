namespace MovieDatabase.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    using static DataConstants.Person;

    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            builder
                .Property(p => p.MiddleName)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            builder
                .Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            builder
                .Property(p => p.Picture)
                .HasMaxLength(PictureMaxLength);
        }
    }
}
