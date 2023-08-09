using Microsoft.AspNetCore.Http;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IFileUploadService
    {
        Task<(UploadFile, string)> UploadFile(List<IFormFile> Files, Modules Module, string Token);
        Task<bool> DeleteFile(Modules Module, string FileName, string Token);
        Task<ApiResponse<List<string>>> FileOperation(List<IFormFile> Files, List<string> DeletedFiles, Modules Module, string Token);
    }
}
