using SolaERP.Infrastructure.Dtos.LogInfo;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface ILogInformationService
    {
        public Task<ApiResponse<List<LogInfoDto>>> GetAllLogInformationAsync(LogInfoGetModel logGetparameters);
    }
}
