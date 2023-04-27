using SixLabors.ImageSharp.Formats.Jpeg;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Enums;
using SolaERP.Infrastructure.Extensions;

namespace SolaERP.Persistence.Services
{
    public class FileService : IFileService
    {
        public async Task<string> DownloadPhotoWithNetworkAsBase64Async(string filePath)
        {
            RemoteFileServer fileServer = new();
            using NetworkConnection networkConnection = new(fileServer.FolderPath, fileServer.Credential);
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

        public async Task<string> UploadBase64PhotoWithNetworkAsync(string base64File, FileExtension extension, string fileName)
        {
            RemoteFileServer fileServer = new();
            using NetworkConnection networkConnection = new(fileServer.FolderPath, fileServer.Credential);
            byte[] imageBytes = Convert.FromBase64String(base64File);

            if (!File.Exists(fileServer.FolderPath))
                Directory.CreateDirectory(fileServer.FolderPath);

            string remoteFilePath = Path.Combine(fileServer.FolderPath, $"{Guid.NewGuid() + fileName}.{extension.ToString()}");
            await File.WriteAllBytesAsync(remoteFilePath, imageBytes);

            return remoteFilePath;
        }
    }
}
