using SolaERP.Application.Dtos.Bid;
using SolaERP.Application.Dtos.BidComparison;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Bid;
using SolaERP.Application.Entities.BidComparison;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Services
{
    public interface IBidComparisonService
    {
        Task<ApiResponse<bool>> SaveBidComparisonAsync(BidComparisonCreateDto bidComparison);
        Task<ApiResponse<bool>> ApproveBidComparisonAsync(BidComparisonApproveDto bidComparisonApprove);
        Task<ApiResponse<bool>> SendComparisonToApproveAsync(BidComparisonSendToApproveDto bidComparisonSendToApprove);
        Task<ApiResponse<List<BidComparisonAllDto>>> GetBidComparisonAllAsync(BidComparisonAllFilterDto filter);
        Task<ApiResponse<BidComparisonDto>> GetBidComparisonAsync(BidComparisonFilterDto bidComparisonApprove);


    }
}
