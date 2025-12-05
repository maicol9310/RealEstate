namespace RealEstate.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IPropertyRepository Properties { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}