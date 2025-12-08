using RealEstate.Domain.Entities;

namespace RealEstate.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
    }
}
