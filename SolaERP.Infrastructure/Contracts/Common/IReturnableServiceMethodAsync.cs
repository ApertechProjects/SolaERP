using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Infrastructure.Contracts.Common
{
    public interface IReturnableServiceMethodAsync<T>
    {
        public Task<ApiResponse<bool>> AddOrUpdateAsync(T requestMainDto);
    }
}
