using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using IFileService = SolaERP.Application.Contracts.Services.IFileService;

namespace SolaERP.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IHostingEnvironment _env;

        public FileService(IHostingEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadAsync(IFormFile file, string filePath, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (file?.Length > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                string fullPath = _env.WebRootPath + Path.Combine(filePath, fileName);

                using FileStream fs = new(fullPath, FileMode.Create);
                await file.CopyToAsync(fs, cancellationToken);

                return fullPath;
            }

            return null;
        }

        public async Task<string> UploadAsync(List<IFormFile> files, string filePath, CancellationToken cancellationToken = default)
        {
            for (int i = 0; i < files.Count; i++)
            {
                await UploadAsync(files[i], filePath, cancellationToken);
            }
            return null;
        }
    }
}
