using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Shared;
using SolaERP.Infrastructure.Extensions;

namespace SolaERP.Infrastructure.Services.Storage
{
    public class LocalStorage : ILocalStorage
    {
        private readonly IOptions<StorageOption> _storageOption;

        public LocalStorage(IOptions<StorageOption> storageOption)
        {
            _storageOption = storageOption;
        }


        public Task DeleteAsync(string pathOrContainerName, string fileName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        //public Task<string> GetAsync(string pathOrContainerName, CancellationToken cancellationToken)
        //{
        //    while (!cancellationToken.IsCancellationRequested)
        //    {
        //        using NetworkConnection _connection = new NetworkConnection(pathOrContainerName, new(_storageOption.Value.Username, _storageOption.Value.Password));

        //    }
        //}

        public async Task<(string fileName, string pathOrContainerName)> UploadAsync(string pathOrContainerName, IFormFile file, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                string filePath = Path.Combine(_storageOption.Value.BaseFolderPath, pathOrContainerName);
                using NetworkConnection _connection = new NetworkConnection(filePath, new(_storageOption.Value.Username, _storageOption.Value.Password));

                string fileName = Path.Combine(Guid.NewGuid().ToString(), Path.GetFileName(file.FileName));
                string fullPath = Path.Combine(filePath, fileName);

                FileStream fs = new(fullPath, FileMode.CreateNew);
                await file.CopyToAsync(fs);

                return (fileName, fullPath);
            }
            return (string.Empty, string.Empty);
        }


        public async Task<string> UploadAsync(byte[] formDatas, string path, string fileName, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                string filePath = Path.Combine(_storageOption.Value.BaseFolderPath, path);
                using NetworkConnection _connection = new NetworkConnection(filePath, new(_storageOption.Value.Username, _storageOption.Value.Password));

                string fullname = Path.Combine(Guid.NewGuid().ToString(), Path.GetFileName(fileName));
                string fullPath = Path.Combine(filePath, fileName);

                await File.WriteAllBytesAsync(fullPath, formDatas);
                return fullPath;
            }
            return string.Empty;
        }
    }
}
