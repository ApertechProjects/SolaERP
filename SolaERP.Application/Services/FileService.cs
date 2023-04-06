using SixLabors.ImageSharp.Formats.Jpeg;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Extensions;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Application.Services
{
    public class FileService : IFileService
    {
        public async Task<string> DownloadPhotoWithNetworkAsBase64Async(string filePath)
        {
            using NetworkConnection networkConnection = new(RemoteFileServer.FolderPath, RemoteFileServer.Credential);
            using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read);
            using MemoryStream memoryStream = new();

            await fileStream.CopyToAsync(memoryStream);

            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public byte[] ResizeImage(byte[] imageData, int width, int height)
        {
            using MemoryStream inputMemoryStream = new(imageData);
            using MemoryStream outPutMemoryStream = new(imageData);

            using Image image = Image.Load(inputMemoryStream);

            image.Mutate(x =>
            {
                x.Resize(new ResizeOptions
                {
                    Size = new(width, height),
                    Mode = ResizeMode.Max
                }); ;
            });

            image.Save(outPutMemoryStream, new JpegEncoder());
            return outPutMemoryStream.ToArray();
        }

        public async Task<string> UploadBase64PhotoWithNetworkAsync(PhotoUploadModel model)
        {
            using NetworkConnection networkConnection = new(RemoteFileServer.FolderPath, RemoteFileServer.Credential);
            byte[] imageBytes = Convert.FromBase64String(model.base64Photo);

            if (!File.Exists(RemoteFileServer.FolderPath))
                Directory.CreateDirectory(RemoteFileServer.FolderPath);

            string remoteFilePath = Path.Combine(RemoteFileServer.FolderPath, $"{Guid.NewGuid() + model.FileName}.{model.Extension.ToString()}");
            await File.WriteAllBytesAsync(remoteFilePath, imageBytes);

            return remoteFilePath;
        }
    }
}
