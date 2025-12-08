using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly RealEstateDbContext _db;

    public UserRepository(RealEstateDbContext db) => _db = db;

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
    {
        return await _db.Users
        .FirstOrDefaultAsync(x => x.Username == username);
    }
}