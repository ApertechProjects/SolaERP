using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Dtos.Request;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IRequestService : IDeleteableAsync, IReadableAsync<RequestMainDto>, IReturnableServiceMethodAsync<RequestMainDto>
    {
        Task<List<RequestMainDto>> GetAllAsync(RequestMainGetParametersDto requestMainGet);
    }
}
