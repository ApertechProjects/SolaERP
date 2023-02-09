using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IItemService
    {
        public Task<ApiResponse<List<ItemCodeDto>>> GetAllAsync();
    }
}
