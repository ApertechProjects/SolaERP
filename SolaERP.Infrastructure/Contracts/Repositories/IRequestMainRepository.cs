using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Entities.Request;
using SolaERP.Infrastructure.Enums;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IRequestMainRepository : IDeleteableAsync, IReturnableRepoMethodAsync<RequestMainSaveModel>
    {
        Task<bool> ChangeRequestStatusAsync(RequestChangeStatusModel requestChangeStatusParametersDto);
        Task<List<RequestMain>> GetAllAsync(int businessUnitId, string itemCode, DateTime dateFrom, DateTime dateTo, ApproveStatuses ApproveStatus, Statuss Status);
        Task<RequestMain> GetRequestByRequestMainIdAsync(int requestMainId);
        Task<List<RequestTypes>> GetRequestTypesByBusinessUnitIdAsync(int businessUnitId);
        Task<List<RequestMain>> GetWaitingForApprovalsAsync(int userId, int businessUnitId, DateTime dateFrom, DateTime dateTo, string itemCode);
        Task<List<RequestMain>> GetWaitingForApprovalsAsync2(int userId, int businessUnitId, DateTime dateFrom, DateTime dateTo, string itemCode);
        Task<List<RequestMainDraft>> GetMainRequestDraftsAsync(int businessUnitId, string itemCode, DateTime dateFrom, DateTime dateTo);
        Task<RequestCardMain> GetRequesMainHeaderAsync(int requestMainId, int userId);
        Task<bool> SendRequestToApproveAsync(int userId, int requestMainId);
        Task<List<RequestApprovalInfo>> GetRequestApprovalInfoAsync(int requestMainId, int userId);
        Task<List<RequestAmendment>> GetApproveAmendmentRequestsAsync(int userId, RequestApproveAmendmentModel requestParametersDto);
        Task<RequestSaveResultModel> AddOrUpdateRequestAsync(int userId, RequestMainSaveModel model);
    }
}
