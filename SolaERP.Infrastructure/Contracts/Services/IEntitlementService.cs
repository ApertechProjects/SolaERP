using SolaERP.Application.Dtos.Entitlement;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Contracts.Services;

public interface IEntitlementService
{
    Task<ApiResponse<bool>> SaveEntitlementsAsync(List<EntitlementUIDDto> data);

    Task<ApiResponse<List<EntitlementListDto>>> GetEntitlementListAsync(int businessUnitId, int periodFrom,
        int periodTo);
}