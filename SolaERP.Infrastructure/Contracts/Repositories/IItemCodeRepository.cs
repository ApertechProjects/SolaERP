using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Dtos.Item_Code;
using SolaERP.Infrastructure.Entities.Item_Code;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IItemCodeRepository : IReadableAsync<ItemCode>
    {
        public Task<ItemCode> GetItemCodeByItemCodeAsync(string itemCode);
        public Task<ItemCodeInfo> GetItemCodeInfoByItemCodeAsync(string itemCode);
        public Task<List<ItemCodeWithImages>> GetItemCodesWithImagesAsync();
    }
}
