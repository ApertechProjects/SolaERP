using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Dtos.Request;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IRequestService : IDeleteableAsync
    {
        Task<ApiResponse<List<RequestMainDto>>> GetAllAsync(RequestMainGetParametersDto requestMainGet);
        Task<ApiResponse<List<RequestWFADto>>> GetWaitingForApprovalsAsync(string finderToken, RequestWFAGetParametersDto requestWFAGetParametersDto);
        Task<bool> RemoveRequestDetailAsync(RequestDetailDto requestDetailDto);
        Task<ApiResponse<List<RequestTypesDto>>> GetRequestTypesByBusinessUnitIdAsync(int businessUnitId);
        Task<ApiResponse<bool>> ChangeRequestStatus(string finderToken, List<RequestChangeStatusModel> changeStatusParametersDtos);
        Task<ApiResponse<bool>> SendMainToApproveAsync(RequestMainSendToApproveDto sendToApproveModel);
        Task<ApiResponse<List<RequestMainDraftDto>>> GetRequestMainDraftsAsync(RequestMainDraftModel getMainDraftParameters);
        Task<ApiResponse<List<RequestApproveAmendmentDto>>> GetApproveAmendmentRequests(string finderToken, RequestApproveAmendmentModel requestParametersDto);
        Task<ApiResponse<RequestCardMainDto>> GetRequestByRequestMainId(string authToken, int requestMainId);
        Task<ApiResponse<bool>> AddOrUpdateAsync(string finderToken, RequestSaveModel model);
        Task<ApiResponse<List<RequestApprovalInfoDto>>> GetRequestApprovalInfoAsync(string finderToken, int requestMainId);
        Task<ApiResponse<RequestMainDto>> GetRequestHeaderAsync(string finderToken, int requestMainId);
        Task<ApiResponse<List<RequestDetailsWithAnalysisCodeDto>>> GetRequestDetails(int requestmainId);
        Task<ApiResponse<RequestSaveResultModel>> AddOrUpdateRequestAsync(string finderToken, RequestSaveModel model);
    }
}
