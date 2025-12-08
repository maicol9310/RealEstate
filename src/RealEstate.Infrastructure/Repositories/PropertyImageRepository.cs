using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Repositories
{
    public class PropertyImageRepository : IPropertyImageRepository
    {
        private readonly RealEstateDbContext _db;

        public PropertyImageRepository(RealEstateDbContext db) => _db = db;
    

        public async Task AddAsync(PropertyImage entity, CancellationToken ct = default)
        {
            await _db.PropertyImages.AddAsync(entity, ct);
        }

        public void Update(PropertyImage entity)
        {
            _db.PropertyImages.Update(entity);
        }

        public async Task<PropertyImage?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.PropertyImages
                .FirstOrDefaultAsync(x => x.IdPropertyImage == id, ct);
        }
    }
}
