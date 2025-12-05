namespace RealEstate.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveBase64FileAsync(string base64, CancellationToken ct = default);
    }
}