using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SolaERP.Application.AppServices.Contract;

namespace SolaERP.Application.AppServices.Concretes;

public class FileService : IFileService
{
    private readonly IHostingEnvironment _environment;

    public FileService(IHostingEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> UploadFileAsync(IFormFile file, string path)
    {
        string fileName = Guid.NewGuid().ToString() + Path.GetFileName(file.FileName);
        string fullPath = Path.Combine(_environment.WebRootPath, path);

        if (file.Length > 0)
        {
            using FileStream fs = new FileStream(fullPath, FileMode.CreateNew);
            await file.CopyToAsync(fs);
        }

        return fullPath;
    }
}