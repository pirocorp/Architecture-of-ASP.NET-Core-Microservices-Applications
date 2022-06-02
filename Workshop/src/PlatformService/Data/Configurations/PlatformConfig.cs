namespace PlatformService.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using PlatformService.Data.Models;

    public class PlatformConfig : IEntityTypeConfiguration<Platform>
    {
        public void Configure(EntityTypeBuilder<Platform> platform)
        {
            platform.HasKey(p => p.Id);

            platform
                .Property(p => p.Id)
                .IsRequired();

            platform
                .Property(p => p.Name)
                .IsRequired();

            platform
                .Property(p => p.Publisher)
                .IsRequired();

            platform
                .Property(p => p.Cost)
                .IsRequired();
        }
    }
}
