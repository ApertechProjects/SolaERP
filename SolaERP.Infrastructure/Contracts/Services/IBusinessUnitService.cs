using SolaERP.Infrastructure.Dtos.BusinessUnit;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IBusinessUnitService : ICrudService<BusinessUnitsAllDto>
    {
        Task<ApiResponse<List<BusinessUnitsDto>>> GetBusinessUnitListByUserId(int userId);
    }
}
