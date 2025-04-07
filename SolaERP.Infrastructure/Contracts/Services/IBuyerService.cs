using SolaERP.Application.Dtos.Buyer;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IBuyerService : ICrudService<BuyerDto>
    {
        Task<ApiResponse<List<BuyerDto>>> GetBuyersAsync(string name, int businessUnitId);

        Task<string> FindBuyerEmailByBuyerName(string buyerName , int businessUnitId);
    }
}
