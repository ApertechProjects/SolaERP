using SolaERP.Application.Dtos.Buyer;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Contracts.Services
{
    public interface IBuyerService : ICrudService<BuyerDto>
    {
        Task<ApiResponse<List<BuyerDto>>> GetBuyersAsync(string name, int businessUnitId);

        Task<string> FindBuyerEmailByBuyerName(string buyerName, int businessUnitId);
        Task<BuyerDto> FindBuyerDataByBuyerName(string buyerName, int businessUnitId);
    }
}