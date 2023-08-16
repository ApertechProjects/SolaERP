using Microsoft.AspNetCore.Http;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IFileUploadService
    {
        Task<(UploadFile, string)> UploadFile(List<IFormFile> files, Modules module, string token);
        Task<bool> DeleteFile(Modules module, string fileName, string token);
        string GetFileLink(string fileName, Modules module, string token);
        Task<ApiResponse<List<string>>> AddFile(List<IFormFile> files, List<string> deletedFiles, Modules module, string token);
    }
}
