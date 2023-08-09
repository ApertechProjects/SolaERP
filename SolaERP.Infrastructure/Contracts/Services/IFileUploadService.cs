using Microsoft.AspNetCore.Http;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IFileUploadService
    {
        Task<bool> UploadFile(List<IFormFile> Files, Modules Module, string BearerToken);
    }
}
