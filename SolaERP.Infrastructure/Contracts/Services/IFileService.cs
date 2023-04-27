using SolaERP.Application.Enums;

namespace SolaERP.Application.Contracts.Services
{
    public interface IFileService
    {
        Task<string> UploadBase64PhotoWithNetworkAsync(string base64File, FileExtension extension, string fileName);
        Task<string> DownloadPhotoWithNetworkAsBase64Async(string fileName);
        byte[] ResizeImage(byte[] imageData, int width, int height);
    }
}
