using SolaERP.Application.Dtos.LogInfo;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface ILogInformationService
    {
        public Task<ApiResponse<List<LogInfoDto>>> GetAllLogInformationAsync(LogInfoGetModel logGetparameters);
    }
}
