using Microsoft.AspNetCore.Http;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IFileUploadService
    {
        Task<(UploadFile, string)> UploadFile(List<IFormFile> files, Modules module);
        Task<bool> DeleteFile(Modules module, string fileName);
        string GetFileLink(string fileName, Modules module);
        string GetDownloadFileLink(string fileName, Modules module);
        Task<ApiResponse<List<string>>> AddFile(List<IFormFile> files, Modules module, List<string> deletedFiles);
        Task<string> GetLinkForEntity(IFormFile formFile, Modules module, bool CheckIsDeleted, string FileLink);

    }
}