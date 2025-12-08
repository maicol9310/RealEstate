using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Persistence.Configurations
{
    public class AppLogConfiguration : IEntityTypeConfiguration<AppLog>
    {
        public void Configure(EntityTypeBuilder<AppLog> builder)
        {
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Level).IsRequired().HasMaxLength(50);
            builder.Property(l => l.Message).IsRequired();
            builder.Property(l => l.Exception).HasMaxLength(4000);
            builder.Property(l => l.Properties).HasMaxLength(4000);
            builder.Property(l => l.Timestamp).IsRequired();
        }
    }
}