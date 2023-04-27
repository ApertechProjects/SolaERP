using SolaERP.Application.Contracts.Common;
using SolaERP.Application.Dtos.Request;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IRequestService : IDeleteableAsync
    {
        Task<ApiResponse<List<RequestMainDto>>> GetAllAsync(RequestMainGetModel requestMainGet);
        Task<ApiResponse<List<RequestWFADto>>> GetWaitingForApprovalsAsync(string name, RequestWFAGetModel requestWFAGetParametersDto);
        Task<bool> RemoveRequestDetailAsync(int requestDetailId);
        Task<ApiResponse<List<RequestTypesDto>>> GetRequestTypesByBusinessUnitIdAsync(int businessUnitId);
        Task<ApiResponse<bool>> RequestMainChangeStatusAsync(string name, RequestChangeStatusModel changeStatusParametersDto);
        Task<ApiResponse<bool>> RequestSendToApproveAsync(string name, int requestMainId);
        Task<ApiResponse<List<RequestMainDraftDto>>> GetRequestMainDraftsAsync(RequestMainDraftModel getMainDraftParameters);
        Task<ApiResponse<List<RequestAmendmentDto>>> GetApproveAmendmentRequests(string name, RequestApproveAmendmentModel requestParametersDto);
        Task<ApiResponse<RequestCardMainDto>> GetRequestByRequestMainId(string name, int requestMainId);
        Task<ApiResponse<List<RequestApprovalInfoDto>>> GetRequestApprovalInfoAsync(string name, int requestMainId);
        Task<ApiResponse<RequestMainDto>> GetRequestHeaderAsync(string name, int requestMainId);
        Task<ApiResponse<List<RequestDetailsWithAnalysisCodeDto>>> GetRequestDetails(int requestmainId);
        Task<ApiResponse<RequestSaveResultModel>> AddOrUpdateRequestAsync(string name, RequestSaveModel model);
        Task<ApiResponse<bool>> DeleteRequestAsync(string name, int requestMainId);
        Task<ApiResponse<List<RequestDetailApprovalInfoDto>>> GetRequestDetailApprvalInfoAsync(int requestDetaildId);
        Task<ApiResponse<NoContentDto>> RequestDetailChangeStatusAsync(string name, RequestDetailApproveModel model);
        Task<ApiResponse<bool>> UpdateBuyerAsync(List<RequestSetBuyer> requestSetBuyer);
        Task<ApiResponse<List<RequestFollowDto>>> RequestFollowUserLoadAsync(int requestMainId);
        Task<ApiResponse<bool>> RequestFollowSaveAsync(RequestFollowSaveModel saveModel);
        Task<ApiResponse<bool>> RequestFollowDeleteAsync(int requestFollowId);
        Task SendFollowMailForRequest(string[] tos, string messageBody, string subject);

    }
}
