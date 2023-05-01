using Microsoft.AspNetCore.Http;

namespace SolaERP.Application.Contracts.Services
{
    public interface IFileService
    {
        Task<string> UploadAsync(IFormFile file, string filePath, CancellationToken cancellationToken = default);
        Task<string> UploadAsync(List<IFormFile> files, string filePath, CancellationToken cancellationToken = default);
    }
}
