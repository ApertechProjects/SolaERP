using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IFileUploadService
    {
        Task<bool> UploadFile(UploadFile uploadFile);
    }
}
