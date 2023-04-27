using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Contracts.Common
{
    public interface IReturnableServiceMethodAsync<T>
    {
        public Task<ApiResponse<bool>> AddOrUpdateAsync(T requestMainDto);
    }
}
