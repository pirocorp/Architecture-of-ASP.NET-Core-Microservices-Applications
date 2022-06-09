namespace CommandService.Data.Configurations
{
    using CommandService.Data.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CommandConfig : IEntityTypeConfiguration<Command>
    {
        public void Configure(EntityTypeBuilder<Command> command)
        {
            command.HasKey(c => c.Id);

            command
                .Property(c => c.Id)
                .IsRequired();

            command
                .Property(c => c.HowTo)
                .IsRequired();

            command
                .Property(c => c.CommandLine)
                .IsRequired();

            command
                .Property(c => c.PlatformId)
                .IsRequired();

            command
                .HasOne(c => c.Platform)
                .WithMany(p => p.Commands)
                .HasForeignKey(c => c.PlatformId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
