using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Persistence
{
    public class RealEstateDbContext : DbContext
    {
        public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options) : base(options) { }

        public DbSet<Property> Properties { get; set; } = null!;
        public DbSet<PropertyImage> PropertyImages { get; set; } = null!;
        public DbSet<PropertyTrace> PropertyTraces { get; set; } = null!;
        public DbSet<Owner> Owners { get; set; } = null!;
        public DbSet<AppLog> AppLogs { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new Configurations.PropertyConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PropertyImageConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PropertyTraceConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.OwnerConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.AppLogConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.UserConfiguration());
        }
    }
}