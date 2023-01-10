namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IRequestService : IDeleteableAsync, IDeleteableByEntityAsync<RequestMain>, IReturnableServiceMethodAsync<RequestMainDto>
    {
        Task<ApiResponse<List<RequestMainDto>>> GetAllAsync(RequestMainGetParametersDto requestMainGet);
        Task<int> SaveRequestDetailsAsync(RequestDetailDto requestDetailDto);
        Task<ApiResponse<RequestSaveVM>> SaveRequest(RequestSaveVM requestSaveVM);
        bool RemoveRequestDetailAsync(RequestDetailDto requestDetailDto);
        Task<List<RequestTypesDto>> GetRequestTypesByBusinessUnitId(int businessUnitId);
    }
}
