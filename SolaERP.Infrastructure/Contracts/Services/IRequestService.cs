using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Dtos.Request;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IRequestService : IDeleteableAsync, IReturnableServiceMethodAsync<RequestMainDto>
    {
        Task<ApiResponse<List<RequestMainDto>>> GetAllAsync(RequestMainGetParametersDto requestMainGet);
        Task<ApiResponse<List<RequestMainWithDetailsDto>>> GetAllMainRequetsWithDetails();
        Task<ApiResponse<RequestMainWithDetailsDto>> GetRequetsMainWithDetailsById(int id);
        Task<bool> RemoveRequestDetailAsync(RequestDetailDto requestDetailDto);
        Task<ApiResponse<List<RequestTypesDto>>> GetRequestTypesByBusinessUnitIdAsync(int businessUnitId);
        Task<ApiResponse<bool>> ChangeRequestStatus(List<RequestChangeStatusParametersDto> changeStatusParametersDtos);
        Task<ApiResponse<bool>> SendMainToApproveAsync(RequestMainSendToApproveDto sendToApproveModel);
        Task<ApiResponse<List<RequestMainDraftDto>>> GetAllRequestMainDraftsAsync(RequestMainDraftGetDto getMainDraftParameters);
    }
}
