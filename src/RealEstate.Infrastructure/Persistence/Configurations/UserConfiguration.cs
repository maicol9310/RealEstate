using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Username).IsRequired().HasMaxLength(200);
            builder.Property(o => o.PasswordHash).HasMaxLength(500);
            builder.Property(o => o.Role).IsRequired();
        }
    }
}
