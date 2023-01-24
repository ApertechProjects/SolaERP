using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Dtos.Request;
using SolaERP.Infrastructure.Entities.Request;
using SolaERP.Infrastructure.Enums;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IRequestMainRepository : IDeleteableAsync, IReturnableRepoMethodAsync<RequestMain>
    {
        Task<bool> ChangeRequestStatusAsync(RequestChangeStatusParametersDto requestChangeStatusParametersDto);
        Task<List<RequestMain>> GetAllAsync(int businessUnitId, string itemCode, DateTime dateFrom, DateTime dateTo, ApproveStatuses ApproveStatus, Status Status);
        Task<RequestMain> GetRequestByRequestMainIAsync(int requestMainId);
        Task<List<RequestTypes>> GetRequestTypesByBusinessUnitIdAsync(int businessUnitId);
        Task<List<RequestMain>> GetWaitingForApprovalsAsync(int userId, int businessUnitId, DateTime dateFrom, DateTime dateTo, string itemCode);
        Task<List<RequestMainDraft>> GetAllMainRequestDraftsAsync(int businessUnitId, string itemCode, DateTime dateFrom, DateTime dateTo);
        Task<bool> SendRequestToApproveAsync(int userId, int requestMainId);
        Task<List<RequestMain>> GetApproveAmendmentRequestsAsync(int userId, RequestApproveAmendmentGetParametersDto requestParametersDto);
    }
}
