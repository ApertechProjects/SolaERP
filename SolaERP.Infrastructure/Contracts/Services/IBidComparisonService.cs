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
        Task<bool> OrderCreateFromApproveBid(CreateOrderFromBidDto entity);
        Task<ApiResponse<bool>> SaveBidComparisonAsync(BidComparisonCreateDto bidComparison);
        Task<ApiResponse<bool>> ApproveBidComparisonsAsync(List<BidComparisonApproveDto> bidComparisonApproves, string userIdentity);
        Task<ApiResponse<bool>> SendComparisonToApproveAsync(BidComparisonSendToApproveDto bidComparisonSendToApprove);
        Task<ApiResponse<List<BidComparisonAllDto>>> GetBidComparisonAllAsync(BidComparisonAllFilterDto filter);
        Task<ApiResponse<BidComparisonDto>> GetBidComparisonAsync(BidComparisonFilterDto bidComparisonApprove);

        Task<ApiResponse<List<BidComparisonWFALoadDto>>> GetComparisonWFA(BidComparisonWFAFilterDto filterDto);
        Task<ApiResponse<List<BidComparisonDraftLoadDto>>> GetComparisonDraft(BidComparisonDraftFilterDto filterDto);
        Task<ApiResponse<List<BidComparisonHeldLoadDto>>> GetComparisonHeld(BidComparisonHeldFilterDto filterDto);
        Task<ApiResponse<List<BidComparisonMyChartsLoadDto>>> GetComparisonMyCharts(BidComparisonMyChartsFilterDto filterDto, string userIdentity);
        Task<ApiResponse<List<BidComparisonNotReleasedLoadDto>>> GetComparisonNotReleased(BidComparisonNotReleasedFilterDto filterDto);
        Task<ApiResponse<List<BidComparisonRejectedLoadDto>>> GetComparisonRejected(BidComparisonRejectedFilterDto filterDto);
        Task<ApiResponse<bool>> HoldBidComparison(HoldBidComparisonRequest request);
    }
}
