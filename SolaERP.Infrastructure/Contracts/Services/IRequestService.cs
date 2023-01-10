namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IRequestService : IDeleteableAsync, IReturnableServiceMethodAsync<RequestMainDto>
    {
        Task<ApiResponse<List<RequestMainDto>>> GetAllAsync(RequestMainGetParametersDto requestMainGet);
        Task<int> SaveRequestDetailsAsync(RequestDetailDto requestDetailDto);
        Task<ApiResponse<RequestSaveVM>> SaveRequest(RequestSaveVM requestSaveVM);

        Task<List<RequestTypesDto>> GetRequestTypesByBusinessUnitId(int businessUnitId);
        Task<bool> RemoveRequestDetailAsync(RequestDetailDto requestDetailDto);
    }
}
