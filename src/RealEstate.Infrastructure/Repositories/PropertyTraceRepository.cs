using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace RealEstate.Infrastructure.Repositories
{
    public class PropertyTraceRepository : IPropertyTraceRepository
    {
        private readonly RealEstateDbContext _db;

        public PropertyTraceRepository(RealEstateDbContext db) => _db = db;


        public async Task AddAsync(PropertyTrace entity, CancellationToken ct = default)
        {
            await _db.PropertyTraces.AddAsync(entity, ct);
        }

        public async Task<PropertyTrace?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.PropertyTraces
                .FirstOrDefaultAsync(x => x.IdPropertyTrace == id, ct);
        }
    }
}
