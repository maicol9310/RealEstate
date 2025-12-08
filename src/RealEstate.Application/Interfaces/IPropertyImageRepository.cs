using RealEstate.Domain.Entities;

namespace RealEstate.Application.Interfaces
{
    public interface IPropertyImageRepository
    {
        Task AddAsync(PropertyImage entity, CancellationToken ct = default);
        Task<PropertyImage?> GetByIdAsync(Guid id, CancellationToken ct = default);
    }
}
