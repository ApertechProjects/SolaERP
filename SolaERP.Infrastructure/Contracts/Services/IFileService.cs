namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IFileService
    {
        Task UploadBase64PhotoToFtpAsync(string base64Photo, string ftpServerAdress, string ftpUserName, string ftpPassword, string outPutFolderPath);
        Task UploadBase64PhotoWithNetworkAsync(string base64Photo, string ftpServerAdress, string ftpUserName, string ftpPassword, string outPutFolderPath);
        byte[] ResizeImage(byte[] imageData, int width, int height);
        Task<byte[]> DownloadPhotoFromFtpAsync(string ftpServerAddress, string ftpUsername, string ftpPassword, string ftpFilePath, string localFolderPath, string fileName);
    }
}
