using SolaERP.Application.Contracts.Common;
using SolaERP.Application.Dtos.Mail;
using SolaERP.Application.Dtos.Request;
using SolaERP.Application.Entities.Request;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IRequestMainRepository : IDeleteableAsync, IReturnableRepoMethodAsync<RequestMainSaveModel>
    {
        Task<List<RequestMainAll>> GetAllAsync(RequestMainGetModel requestMain, int userId);
        Task<List<RequestHeld>> GetHeldAsync(RequestWFAGetModel requestMain, int userId);
        Task<RequestMain> GetRequestByRequestMainIdAsync(int requestMainId);
        Task<List<RequestTypes>> GetRequestTypesByBusinessUnitIdAsync(int businessUnitId);
        Task<List<RequestMain>> GetWaitingForApprovalsAsync(int userId, RequestWFAGetModel requestWFA);
        Task<List<RequestMainDraft>> GetMainRequestDraftsAsync(RequestMainDraftModel requestMain, int userId);
        Task<RequestCardMain> GetRequesMainHeaderAsync(int requestMainId, int userId);
        Task<bool> SendRequestToApproveAsync(int userId, int requestMainId);
        Task<List<RequestApprovalInfo>> GetRequestApprovalInfoAsync(int requestMainId, int userId);

        Task<List<RequestAmendment>> GetApproveAmendmentRequestsAsync(int userId,
            RequestApproveAmendmentModel requestParametersDto);

        Task<RequestSaveResultModel> AddOrUpdateRequestAsync(int userId, RequestMainSaveModel model);
        Task<bool> UpdateBuyerAsync(RequestSetBuyer setBuyers, int userId);
        Task<List<RequestFollow>> RequestFollowUserLoadAsync(int requestMainId);
        Task<bool> RequestFollowSaveAsync(RequestFollowSaveModel saveModel);
        Task<bool> RequestFollowDeleteAsync(int requestFollowId);

        Task<bool> RequestMainChangeStatusAsync(int userId, int requestMainId, int approveStatus, string comment,
            int rejectReasonId);

        Task<bool> RequestFollowCheckUserExistAsync(RequestFollowSaveModel saveModel);
        Task<int> GetDefaultApprovalStage(string keyCode, int businessUnitId);
        Task<List<RequestCategory>> CategoryList(int businessUnitId, string keyCode);
        Task<List<int>> GetDetailIds(int requestMainId);
        Task<List<BuyersAssignment>> GetBuyersAssignment(RequestWFAGetModel model, int userId);
        Task<bool> Retrieve(int requestMainId, int userId);
		Task<RequestMailDto> RequestEmailSendUsers(int requestMainId);
	}
}