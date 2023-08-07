using SolaERP.Application.Dtos.Bid;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Services
{
    public interface IBidService
    {
        Task<ApiResponse<List<BidAllDto>>> GetAllAsync(BidAllFilter filter);
        Task<ApiResponse<int>> SaveBidMainAsync(BidMainDto bidMain);

    }
}
