using SolaERP.Application.Contracts.Common;
using SolaERP.Application.Dtos.Item_Code;
using SolaERP.Application.Entities.Item_Code;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IItemCodeRepository : IReadableAsync<ItemCode>
    {
        public Task<ItemCodeWithImages> GetItemCodeByItemCodeAsync(string businessUnitCode, string itemCode);
        public Task<ItemCodeInfo> GetItemCodeInfoByItemCodeAsync(string itemCode);
        public Task<List<ItemCodeWithImages>> GetItemCodesWithImagesAsync();
        public Task<List<ItemCode>> GetAllAsync(string businessUnitCode);
    }
}
