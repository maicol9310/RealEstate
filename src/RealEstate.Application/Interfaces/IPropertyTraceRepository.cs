using RealEstate.Domain.Entities;

namespace RealEstate.Application.Interfaces
{
    public interface IPropertyTraceRepository
    {
        Task AddAsync(PropertyTrace entity, CancellationToken ct = default);
        Task<PropertyTrace?> GetByIdAsync(Guid id, CancellationToken ct = default);
    }
}
