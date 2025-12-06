using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly RealEstateDbContext _context;

        public OwnerRepository(RealEstateDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Owner owner, CancellationToken ct = default)
        {
            await _context.Owners.AddAsync(owner, ct);
        }
    }
}
