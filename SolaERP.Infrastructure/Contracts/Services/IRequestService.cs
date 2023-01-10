using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Dtos.Request;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.ViewModels;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IRequestService : IDeleteableAsync, IReadableAsync<RequestMainDto>, IReturnableServiceMethodAsync<RequestMainDto>
    {
        Task<int> SaveRequestDetailsAsync(RequestDetailDto requestDetailDto);
        Task<ApiResponse<RequestSaveVM>> SaveRequest(RequestSaveVM requestSaveVM);
        bool RemoveRequestDetailAsync(RequestDetailDto requestDetailDto);
        Task<List<RequestTypesDto>> GetRequestTypesByBusinessUnitId(int businessUnitId);
    }
}
