using SolaERP.Application.Contracts.Common;
using SolaERP.Application.Dtos;
using SolaERP.Application.Dtos.Item_Code;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Contracts.Services
{
    public interface IItemService
    {
        public Task<ApiResponse<List<ItemCodeDto>>> GetAllAsync(int businessUnitId);
        Task<ApiResponse<ItemCodeWithImagesDto>> GetItemCodeByItemCodeAsync(string businessUnitCode, string itemCode);
        Task<ApiResponse<ItemCodeInfoDto>> GetItemCodeInfoByItemCodeAsync(string itemCode,int businessUnitId);
        public Task<ApiResponse<List<ItemCodeWithImagesDto>>> GetItemCodesWithImagesAsync();
    }
}
