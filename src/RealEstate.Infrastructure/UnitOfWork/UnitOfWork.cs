using RealEstate.Application.Interfaces;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly RealEstateDbContext _db;
        public IPropertyRepository Properties { get; }
        public IOwnerRepository Owners { get; }  

        public UnitOfWork(
            RealEstateDbContext db,
            IPropertyRepository propertyRepository,
            IOwnerRepository ownerRepository) 
        {
            _db = db;
            Properties = propertyRepository;
            Owners = ownerRepository;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);

        public void Dispose() => _db.Dispose();
    }
}