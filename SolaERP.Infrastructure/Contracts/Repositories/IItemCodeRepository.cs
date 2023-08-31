using SolaERP.Application.Contracts.Common;
using SolaERP.Application.Dtos.Item_Code;
using SolaERP.Application.Entities.Item_Code;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IItemCodeRepository 
    {
        public Task<ItemCodeWithImages> GetItemCodeByItemCodeAsync(string businessUnitCode, string itemCode);
        public Task<ItemCodeInfo> GetItemCodeInfoByItemCodeAsync(string itemCode,int businessUnitId);
        public Task<List<ItemCodeWithImages>> GetItemCodesWithImagesAsync();
        public Task<List<ItemCode>> GetAllAsync(int businessUnitId);
    }
}
