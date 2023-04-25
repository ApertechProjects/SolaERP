using Microsoft.AspNetCore.Http;

namespace SolaERP.Application.AppServices.Contract
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string path);
    }
}
