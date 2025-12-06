using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Persistence
{
    public class RealEstateDbContext : DbContext
    {
        public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options) : base(options) {}

        public DbSet<Property> Properties { get; set; } = null!;
        public DbSet<PropertyImage> PropertyImages { get; set; } = null!;
        public DbSet<PropertyTrace> PropertyTraces { get; set; } = null!;
        public DbSet<Owner> Owners { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>(b =>
            {
                b.HasKey(x => x.IdProperty);
                b.Property(x => x.Name).HasMaxLength(200).IsRequired();
                b.Property(x => x.Address).HasMaxLength(500);
                b.Property(x => x.Price).HasColumnType("decimal(18,2)");
                b.Property(x => x.CodeInternal).HasMaxLength(100);
                b.HasMany<PropertyImage>().WithOne().HasForeignKey(i => i.IdProperty).OnDelete(DeleteBehavior.Cascade);
                b.HasMany<PropertyTrace>().WithOne().HasForeignKey(t => t.IdProperty).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.Owner).WithMany().HasForeignKey(x => x.IdOwner);
            });

            modelBuilder.Entity<PropertyImage>(b => { b.HasKey(x => x.IdPropertyImage); b.Property(x => x.File).HasMaxLength(1000); });
            modelBuilder.Entity<PropertyTrace>(b => { b.HasKey(x => x.IdPropertyTrace); b.Property(x => x.Value).HasColumnType("decimal(18,2)"); b.Property(x => x.Tax).HasColumnType("decimal(18,2)"); });
            modelBuilder.Entity<Owner>(b => { b.HasKey(x => x.IdOwner); b.Property(x => x.Name).HasMaxLength(200); });
        }
    }
}
