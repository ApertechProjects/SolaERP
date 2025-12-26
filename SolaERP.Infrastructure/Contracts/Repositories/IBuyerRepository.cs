using SolaERP.Application.Entities.Buyer;
using SolaERP.Application.Dtos.Buyer;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IBuyerRepository : ICrudOperations<Buyer>
    {
        public Task<List<Buyer>> GetBuyersAsync(int userId, int businessUnitId);

        public Task<string> FindBuyerEmailByBuyerName(string buyerName, int businessUnitId);
        public Task<string> FindBusinessUnitNameByBuyerName(string buyerName, int businessUnitId);

        public Task<BuyerDto> FindBuyerDataByBuyerName(string buyerName, int businessUnitId);
    }
}
