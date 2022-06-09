namespace CommandService.Data.Configurations
{
    using CommandService.Data.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PlatformConfig : IEntityTypeConfiguration<Platform>
    {
        public void Configure(EntityTypeBuilder<Platform> platform)
        {
            platform.HasKey(p => p.Id);

            platform
                .Property(p => p.Id)
                .IsRequired();

            platform
                .Property(p => p.ExternalId)
                .IsRequired();

            platform
                .Property(p => p.Name)
                .IsRequired();
        }
    }
}
