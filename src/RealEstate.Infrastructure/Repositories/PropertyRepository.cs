using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly RealEstateDbContext _ctx;
        public PropertyRepository(RealEstateDbContext ctx) => _ctx = ctx;

        public async Task AddAsync(Property entity, CancellationToken ct = default) => await _ctx.Properties.AddAsync(entity, ct);

        public async Task<Property?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            await _ctx.Properties.Include(p => p.Images).Include(p => p.Traces).Include(p => p.Owner).FirstOrDefaultAsync(p => p.IdProperty == id, ct);

        public void Update(Property entity) => _ctx.Properties.Update(entity);

        public async Task<IEnumerable<Property>> ListAsync(RealEstate.Application.Interfaces.PropertyFilter filter, CancellationToken ct = default)
        {
            var q = _ctx.Properties.Include(p => p.Owner).Include(p => p.Images).Include(p => p.Traces).AsQueryable();

            if (filter.MinPrice.HasValue) q = q.Where(x => x.Price >= filter.MinPrice.Value);
            if (filter.MaxPrice.HasValue) q = q.Where(x => x.Price <= filter.MaxPrice.Value);
            if (filter.OwnerId.HasValue) q = q.Where(x => x.IdOwner == filter.OwnerId.Value);
            if (filter.Year.HasValue) q = q.Where(x => x.Year == filter.Year.Value);
            if (!string.IsNullOrWhiteSpace(filter.Search)) q = q.Where(x => x.Name.Contains(filter.Search) || x.Address.Contains(filter.Search) || x.CodeInternal.Contains(filter.Search));

            return await q.OrderByDescending(x => x.Price).ToListAsync(ct);
        }
    }
}
