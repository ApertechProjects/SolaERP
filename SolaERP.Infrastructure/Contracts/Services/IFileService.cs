using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IFileService
    {
        Task<string> UploadBase64PhotoWithNetworkAsync(PhotoUploadModel model);
        Task<string> DownloadPhotoWithNetworkAsBase64Async(string fileName);
        byte[] ResizeImage(byte[] imageData, int width, int height);
    }
}
