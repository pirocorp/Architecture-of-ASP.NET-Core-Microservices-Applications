namespace CarRentalSystem.Common.Data.Configuration
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder
                .HasKey(m => m.Id);

            // In database store serializedData and use serializedData field.
            builder
                .Property<string>("serializedData")
                .IsRequired()
                .HasField("serializedData");

            builder
                .Property(m => m.Type)
                .IsRequired()
                // uses lambda expression to convert property to and from database
                .HasConversion(
                    // property is stored as string as AssemblyQualifiedName of the type
                    t => t.AssemblyQualifiedName,
                    // gets the type from AssemblyQualifiedName string stored in the database
                    t => Type.GetType(t));
        }
    }
}
