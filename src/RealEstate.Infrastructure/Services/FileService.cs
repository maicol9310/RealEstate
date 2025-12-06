using RealEstate.Application.Interfaces;
using Microsoft.Extensions.Hosting;

namespace RealEstate.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IHostEnvironment _env;
        public FileService(IHostEnvironment env) => _env = env;

        public async Task<string> SaveBase64FileAsync(string base64, CancellationToken ct = default)
        {
            var data = Convert.FromBase64String(base64);
            var folder = Path.Combine(_env.ContentRootPath, "wwwroot", "images");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var name = $"{Guid.NewGuid()}.jpg";
            var full = Path.Combine(folder, name);

            await File.WriteAllBytesAsync(full, data, ct);

            return $"/images/{name}";
        }
    }
}
