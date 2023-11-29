using SolaERP.Application.Dtos.BusinessUnit;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Contracts.Services
{
    public interface IBusinessUnitService : ICrudService<BusinessUnitsAllDto>
    {
        Task<ApiResponse<List<BaseBusinessUnitDto>>> GetBusinessUnitListByUserToken(string finderToken);
        Task<ApiResponse<List<BaseBusinessUnitDto>>> GetBusinessUnitListByCurrentUser(string identityName);
        Task<List<BusinessUnitConnectionDto>> GetBusinessUnitListConnections();
        Task<string> GetBusinessUnitCode(int businessUnitId);
        Task<string> GetBusinessUnitName(int businessUnitId);
    }
}