using SolaERP.Infrastructure.Dtos.Buyer;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IBuyerService : ICrudService<BuyerDto>
    {
        Task<ApiResponse<List<BuyerDto>>> GetBuyerByUserTokenAsync(string authToken, string businessUnitCode);
     
    }
}
