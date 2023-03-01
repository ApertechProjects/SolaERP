using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Dtos.Request;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IRequestService : IDeleteableAsync
    {
        Task<ApiResponse<List<RequestMainDto>>> GetAllAsync(RequestMainGetModel requestMainGet);
        Task<ApiResponse<List<RequestWFADto>>> GetWaitingForApprovalsAsync(string finderToken, RequestWFAGetModel requestWFAGetParametersDto);
        Task<bool> RemoveRequestDetailAsync(RequestDetailDto requestDetailDto);
        Task<ApiResponse<List<RequestTypesDto>>> GetRequestTypesByBusinessUnitIdAsync(int businessUnitId);
        Task<ApiResponse<bool>> RequestMainChangeStatusAsync(string finderToken, RequestChangeStatusModel changeStatusParametersDto);
        Task<ApiResponse<bool>> RequestSendToApproveAsync(string authToken, int requestMainId);
        Task<ApiResponse<List<RequestMainDraftDto>>> GetRequestMainDraftsAsync(RequestMainDraftModel getMainDraftParameters);
        Task<ApiResponse<List<RequestAmendmentDto>>> GetApproveAmendmentRequests(string finderToken, RequestApproveAmendmentModel requestParametersDto);
        Task<ApiResponse<RequestCardMainDto>> GetRequestByRequestMainId(string authToken, int requestMainId);
        Task<ApiResponse<List<RequestApprovalInfoDto>>> GetRequestApprovalInfoAsync(string finderToken, int requestMainId);
        Task<ApiResponse<RequestMainDto>> GetRequestHeaderAsync(string finderToken, int requestMainId);
        Task<ApiResponse<List<RequestDetailsWithAnalysisCodeDto>>> GetRequestDetails(int requestmainId);
        Task<ApiResponse<RequestSaveResultModel>> AddOrUpdateRequestAsync(string finderToken, RequestSaveModel model);
        Task<ApiResponse<bool>> DeleteRequestAsync(string authToken, int requestMainId);
        Task<ApiResponse<RequestDetailApprovalInfoDto>> GetRequestDetailApprvalInfoAsync(int requestDetaildId);
        Task<ApiResponse<NoContentDto>> RequestDetailChangeStatusAsync(string finderToken, RequestDetailApproveModel model);
        Task<ApiResponse<bool>> UpdateBuyerAsync(RequestSetBuyer requestSetBuyer);
        Task<ApiResponse<List<RequestFollowDto>>> RequestFollowUserLoadAsync(int requestMainId);
        Task<ApiResponse<bool>> AddOrUpdateUserForRequestFollowAsync(List<RequestFollowSaveModel> saveModel);
        Task<ApiResponse<bool>> DeleteUserForRequestFollowAsync(List<RequestFollowSaveModel> saveModel);
        Task SendFollowMailForRequest(string[] tos, string messageBody, string subject);

    }
}
