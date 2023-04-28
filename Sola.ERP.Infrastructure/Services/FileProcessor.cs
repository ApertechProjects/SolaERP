using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using SolaERP.Infrastructure.Contracts;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Services
{
    public class FileProcessor : IFileProcessor
    {
        private readonly IHostingEnvironment _env;
        private readonly IOptions<Configurations.FileOptions> _option;

        public FileProcessor(IOptions<Configurations.FileOptions> option, IHostingEnvironment env)
        {
            _option = option;
            _env = env;
        }

        public Task<string> RenameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadAsync(FileModel file, string path, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string fileName = $"{Guid.NewGuid()}{file.Filename}.{file.Extension.ToString()}";
            string fullPath = Path.Combine(_env.WebRootPath, path, @"\", fileName);

            await File.WriteAllBytesAsync(fullPath, file.Data, cancellationToken);
            return fullPath;
        }
    }
}
