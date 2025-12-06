using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Persistence.Configurations
{
    public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.HasKey(o => o.IdOwner);
            builder.Property(o => o.Name).IsRequired().HasMaxLength(200);
            builder.Property(o => o.Address).HasMaxLength(500);
            builder.Property(o => o.Photo).HasMaxLength(500);
            builder.Property(o => o.Birthday).IsRequired();
        }
    }
}