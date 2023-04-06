using SixLabors.ImageSharp.Formats.Jpeg;
using SolaERP.Infrastructure.Contracts.Services;
using System.Net;

namespace SolaERP.Application.Services
{
    public class FileService : IFileService
    {
        public async Task<byte[]> DownloadPhotoFromFtpAsync(string ftpServerAddress, string ftpUsername, string ftpPassword, string ftpFilePath, string localFolderPath, string fileName)
        {
            FtpWebRequest ftpWebRequest = WebRequest.Create(new Uri($"{ftpServerAddress}/{ftpFilePath}/{fileName}")) as FtpWebRequest;
            ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            ftpWebRequest.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

            using FtpWebResponse ftpWebResponse = await ftpWebRequest.GetResponseAsync() as FtpWebResponse;
            using Stream ftpWebResponseStream = ftpWebResponse.GetResponseStream();
            using MemoryStream memoryStream = new();

            await ftpWebResponseStream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
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

        public async Task UploadBase64PhotoToFtpAsync(string base64Photo, string ftpServerAdress, string ftpUserName, string ftpPassword, string outPutFolderPath)
        {
            byte[] imageBytes = Convert.FromBase64String(base64Photo);
            var uri = string.Format("{0}/{1}", ftpServerAdress, outPutFolderPath);
            FtpWebRequest ftpWebRequest = WebRequest.Create(new Uri("ftp://" + string.Format("{0}", ftpServerAdress))) as FtpWebRequest;
            ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftpWebRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
            ftpWebRequest.KeepAlive = true;

            using Stream requestStream = ftpWebRequest.GetRequestStream();
            requestStream.Write(imageBytes, 0, imageBytes.Length);

            using FtpWebResponse ftResponse = await ftpWebRequest.GetResponseAsync() as FtpWebResponse;
            Console.WriteLine($"Upload File Complete, status: {ftResponse.StatusDescription}");
        }

        //public async Task UploadBase64PhotoToFtpAsync(string base64Photo, string ftpServerAdress, string ftpUserName, string ftpPassword, string outPutFolderPath)
        //{

        //}



        public async Task UploadBase64PhotoWithNetworkAsync(string base64Photo, string ftpServerAdress, string ftpUserName, string ftpPassword, string outPutFolderPath)
        {
            //\\192.168.1.102\g$" + "\\"
            NetworkConnection networkConnection = new(@"\\116.203.90.202\shared folder Dev", new(ftpServerAdress, ftpPassword));
            byte[] imageBytes = Convert.FromBase64String(base64Photo);

            using FileStream stream = new(@"\\116.203.90.202\shared folder Dev", FileMode.Create, FileAccess.ReadWrite);
            await stream.WriteAsync(imageBytes);
        }
    }
}
