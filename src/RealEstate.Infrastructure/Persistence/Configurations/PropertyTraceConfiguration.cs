using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Persistence.Configurations
{
    public class PropertyTraceConfiguration : IEntityTypeConfiguration<PropertyTrace>
    {
        public void Configure(EntityTypeBuilder<PropertyTrace> builder)
        {
            builder.HasKey(pt => pt.IdPropertyTrace);
            builder.Property(pt => pt.Name).IsRequired().HasMaxLength(200);
            builder.Property(pt => pt.Value).IsRequired();
            builder.Property(pt => pt.Tax).IsRequired();
        }
    }
}