using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.Item_Code;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IItemService
    {
        public Task<ApiResponse<List<ItemCodeDto>>> GetAllAsync();
        Task<ApiResponse<ItemCodeWithImagesDto>> GetItemCodeByItemCodeAsync(string itemCode);
        Task<ApiResponse<ItemCodeInfoDto>> GetItemCodeInfoByItemCodeAsync(string itemCode);
        public Task<ApiResponse<List<ItemCodeWithImagesDto>>> GetItemCodesWithImagesAsync();
    }
}
