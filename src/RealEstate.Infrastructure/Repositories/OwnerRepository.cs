using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly RealEstateDbContext _db;

        public OwnerRepository(RealEstateDbContext db) => _db = db;

        public async Task AddAsync(Owner owner, CancellationToken ct = default)
        {
            await _db.Owners.AddAsync(owner, ct);
        }
    }
}
