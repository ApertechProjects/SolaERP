using Microsoft.AspNetCore.Http;

namespace SolaERP.Application.Contracts.Services
{
    public interface IStorage
    {
        Task<(string fileName, string pathOrContainerName)> UploadAsync(string pathOrContainerName, IFormFile file, CancellationToken cancellationToken);
        Task DeleteAsync(string pathOrContainerName, string fileName, CancellationToken cancellationToken);
        List<string> GetFilesAsync();
        Task<bool> CopyToAsync(string path, IFormFile file, CancellationToken cancellationToken);
        bool HasFile(string pathOrContainerName, string fileName, CancellationToken cancellationToken);
        Task<string> UploadAsync(byte[] formDatas, string path, string fileName, CancellationToken cancellationToken);
    }
}
