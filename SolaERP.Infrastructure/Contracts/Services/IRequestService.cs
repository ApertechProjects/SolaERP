using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Dtos.Request;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.ViewModels;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IRequestService : IDeleteableAsync, IReturnableServiceMethodAsync<RequestMainDto>
    {
        Task<ApiResponse<List<RequestMainDto>>> GetAllAsync(RequestMainGetParametersDto requestMainGet);
        Task<ApiResponse<RequestSaveVM>> SaveRequest(RequestSaveVM requestSaveVM);
        Task<bool> RemoveRequestDetailAsync(RequestDetailDto requestDetailDto);
    }
}
