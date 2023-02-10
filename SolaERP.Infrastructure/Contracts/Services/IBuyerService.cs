using SolaERP.Infrastructure.Dtos.Buyer;
using SolaERP.Infrastructure.Dtos.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IBuyerService : ICrudService<BuyerDto>
    {
        Task<ApiResponse<List<BuyerDto>>> GetBuyerByUserTokenAsync(string authToken);
    }
}
