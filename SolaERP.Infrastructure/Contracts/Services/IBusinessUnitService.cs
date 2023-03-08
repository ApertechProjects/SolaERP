using SolaERP.Infrastructure.Dtos.BusinessUnit;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IBusinessUnitService : ICrudService<BusinessUnitsAllDto>
    {
        Task<ApiResponse<List<BaseBusinessUnitDto>>> GetBusinessUnitListByUserToken(string finderToken);
        Task<ApiResponse<List<BusinessUnitForGroupDto>>> GetBusinessUnitForGroupAsync(int groupId);
    }
}
