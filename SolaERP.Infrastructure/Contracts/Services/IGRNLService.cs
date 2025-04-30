using SolaERP.Application.Dtos.GNRLConfig;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Contracts.Services
{
    public interface IGNRLConfigService
    {
        Task<ApiResponse<List<GNRLConfigDto>>> GetGNRLListByBusinessUnitId(int businessUnitId);
    }
}