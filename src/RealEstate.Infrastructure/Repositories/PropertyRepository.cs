using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using RealEstate.Application.Queries;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly RealEstateDbContext _db;
        public PropertyRepository(RealEstateDbContext db) => _db = db;

        public async Task AddAsync(Property entity, CancellationToken ct = default)
        {
            await _db.Properties.AddAsync(entity, ct);
        }

        public void Update(Property entity)
        {
            _db.Properties.Update(entity);
        }

        public async Task<Property?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.Properties
                .Include(p => p.Images)
                .Include(p => p.Traces)
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.IdProperty == id, ct);
        }

        public async Task<IEnumerable<Property>> ListAsync(PropertyFilter filter, CancellationToken ct = default)
        {
            var query = _db.Properties.AsQueryable();
            if (filter.MinPrice is not null) query = query.Where(p => p.Price >= filter.MinPrice);
            if (filter.MaxPrice is not null) query = query.Where(p => p.Price <= filter.MaxPrice);
            if (filter.OwnerId is not null) query = query.Where(p => p.IdOwner == filter.OwnerId);
            if (filter.Year is not null) query = query.Where(p => p.Year == filter.Year);
            return await query.Include(p => p.Images).Include(p => p.Owner).ToListAsync(ct);
        }
    }
}