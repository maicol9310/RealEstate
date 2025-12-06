using RealEstate.Domain.Entities;

namespace RealEstate.Application.Interfaces
{
    public interface IOwnerRepository
    {
        Task AddAsync(Owner owner, CancellationToken ct = default);
    }
}
