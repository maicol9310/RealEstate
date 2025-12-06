using RealEstate.Application.Interfaces;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RealEstateDbContext _ctx;
        public IPropertyRepository Properties { get; }

        public UnitOfWork(RealEstateDbContext ctx, IPropertyRepository propertyRepository)
        {
            _ctx = ctx;
            Properties = propertyRepository;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _ctx.SaveChangesAsync(ct);
    }
}