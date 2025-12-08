namespace RealEstate.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IPropertyRepository Properties { get; }
        IOwnerRepository Owners { get; }
        IPropertyImageRepository PropertyImage { get; }
        IPropertyTraceRepository PropertyTrace { get; }
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}