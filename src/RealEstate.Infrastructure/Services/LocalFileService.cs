using Microsoft.Extensions.Configuration;
using RealEstate.Application.Interfaces;

namespace RealEstate.Infrastructure.Services
{
    public class LocalFileService : IFileService
    {
        private readonly string _basePath;

        private static readonly Dictionary<string, string> MimeToExt = new()
        {
            { "image/jpeg", ".jpg" },
            { "image/jpg", ".jpg" },
            { "image/png", ".png" },
            { "image/webp", ".webp" },
            { "image/gif", ".gif" }
        };

        public LocalFileService(IConfiguration configuration)
        {
            _basePath = configuration.GetSection("FileStorage")["BasePath"] ?? "./uploads";
            if (!Directory.Exists(_basePath)) Directory.CreateDirectory(_basePath);
        }

        public async Task<string> SaveBase64FileAsync(string base64, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(base64))
                throw new ArgumentException("Base64 data required", nameof(base64));

            string mime = string.Empty;
            string data = base64;

            if (base64.Contains(","))
            {
                var parts = base64.Split(',');
                var header = parts[0];   
                data = parts[1];

                if (header.Contains(";base64"))
                {
                    mime = header.Replace("data:", "").Replace(";base64", "").Trim();
                }
            }

            var ext = MimeToExt.ContainsKey(mime) ? MimeToExt[mime] : ".png"; 
            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(_basePath, fileName);

            var bytes = Convert.FromBase64String(data);
            await File.WriteAllBytesAsync(fullPath, bytes, ct);

            return fileName; 
        }
    }
}