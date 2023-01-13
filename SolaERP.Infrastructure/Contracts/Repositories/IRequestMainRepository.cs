using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Entities.Request;
using SolaERP.Infrastructure.Enums;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IRequestMainRepository : IDeleteableAsync, IReturnableRepoMethodAsync<RequestMain>
    {
        Task<List<RequestMain>> GetAllAsync(int businessUnitId, string itemCode, DateTime dateFrom, DateTime dateTo, ApproveStatuses ApproveStatus, Status Status);
        Task<List<RequestTypes>> GetRequestTypesByBusinessUnitId(int businessUnitId);
    }
}
