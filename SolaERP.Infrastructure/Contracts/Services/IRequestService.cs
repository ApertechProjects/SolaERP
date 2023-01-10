using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Dtos.Request;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IRequestService : IDeleteableAsync, IReturnableServiceMethodAsync<RequestMainDto>
    {
        Task<ApiResponse<List<RequestMainDto>>> GetAllAsync(RequestMainGetParametersDto requestMainGet);
        Task<bool> SaveRequestDetailsAsync(RequestDetailDto requestDetailDto);
        //Task<ApiResponse<RequestSaveVM>> SaveRequest(RequestSaveVM requestSaveVM);

        Task<List<RequestTypesDto>> GetRequestTypesByBusinessUnitId(int businessUnitId);
        Task<bool> RemoveRequestDetailAsync(RequestDetailDto requestDetailDto);
    }
}
