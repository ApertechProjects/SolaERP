using SolaERP.Application.Dtos.Bid;
using SolaERP.Application.Dtos.BidComparison;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Bid;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Services
{
    public interface IBidComparionService
    {
        Task<ApiResponse<bool>> SaveBidComparisonAsync(BidComparisonCreateDto bidComparison);
        Task<ApiResponse<bool>> ApproveBidComparisonAsync(BidComparisonApproveDto bidComparisonApprove);


    }
}
