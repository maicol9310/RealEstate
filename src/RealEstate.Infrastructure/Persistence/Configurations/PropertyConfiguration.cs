using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Persistence.Configurations
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.HasKey(p => p.IdProperty);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Address).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.CodeInternal).HasMaxLength(100);
            builder.Property(p => p.Year).IsRequired();

            // Navigation access mode
            builder.Metadata.FindNavigation(nameof(Property.Images))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Property.Traces))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            // ✔ RELACIONES CORRECTAS
            builder.HasMany(p => p.Images)
                   .WithOne(pi => pi.Property)
                   .HasForeignKey(pi => pi.IdProperty)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Traces)
                   .WithOne(pt => pt.Property)
                   .HasForeignKey(pt => pt.IdProperty)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Owner)
                   .WithMany(o => o.Properties)
                   .HasForeignKey(p => p.IdOwner)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}