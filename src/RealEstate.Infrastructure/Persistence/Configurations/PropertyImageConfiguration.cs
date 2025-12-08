using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Persistence.Configurations
{
    public class PropertyImageConfiguration : IEntityTypeConfiguration<PropertyImage>
    {
        public void Configure(EntityTypeBuilder<PropertyImage> builder)
        {
            builder.HasKey(pi => pi.IdPropertyImage);

            builder.Property(pi => pi.File).IsRequired();

            builder.Property(pi => pi.Enabled)
                   .HasDefaultValue(true);
            
            builder.HasOne(pi => pi.Property)
                   .WithMany(p => p.Images)
                   .HasForeignKey(pi => pi.IdProperty)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}