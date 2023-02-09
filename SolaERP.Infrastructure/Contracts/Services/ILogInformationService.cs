using SolaERP.Infrastructure.Dtos.LogInfo;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface ILogInformationService
    {
        public Task<ApiResponse<LogInfoDto>> GetSingleLogInformationAsync(LogInfoGetDto logGetparameters);
        public Task<ApiResponse<List<LogInfoDto>>> GetAllLogInformationAsync(LogInfoGetDto logGetparameters);
    }
}
