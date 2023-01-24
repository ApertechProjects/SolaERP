using SolaERP.Infrastructure.Dtos.Request;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Infrastructure.Contracts.Common
{
    public interface IReturnableServiceMethodAsync<T>
    {
        public Task<ApiResponse<RequestMainDto>> AddOrUpdateAsync(T requestMainDto);
    }
}
