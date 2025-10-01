using SolaERP.Application.Dtos.Entitlement;

namespace SolaERP.Application.Contracts.Repositories;

public interface IEntitlementRepository
{
    Task<bool> SaveEntitlementIUD(EntitlementUIDDto dto);
    Task<List<EntitlementListDto>> GetEntitlementsList(int businessUnitId, int periodFrom, int periodTo);
}