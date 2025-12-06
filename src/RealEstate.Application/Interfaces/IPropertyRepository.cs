using RealEstate.Application.Queries;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Interfaces
{
    public interface IPropertyRepository
    {
        Task<Property?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(Property entity, CancellationToken ct = default);
        void Update(Property entity);
        Task<IEnumerable<Property>> ListAsync(PropertyFilter filter, CancellationToken ct = default);
    }
}