using SolaERP.Application.Dtos.BidComparison;
using SolaERP.Application.Entities.Bid;
using SolaERP.Application.Entities.BidComparison;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IBidComparisonRepository
    {
        Task<int> AddComparisonAsync(BidComparisonIUD entity);
        Task<bool> SaveComparisonBids(int bidComparisonId, DataTable dataTable);
        Task<bool> ApproveComparisonAsync(BidComparisonApprove entity);
        Task<bool> OrderCreateFromApproveBid(CreateOrderFromBidDto entity);
        Task<bool> SendComparisonToApprove(BidComparisonSendToApprove filter);
        Task<bool> BidApprove(BidComparisonBidApproveDto dto, int UserId);
        Task BidReject(BidComparisonBidRejectDto dto, int UserId);
        Task<List<BidComparisonAll>> GetComparisonAll(BidComparisonAllFilter filter);
        Task<List<BidComparisonBidApprovalsLoad>> GetComparisonBidApprovals(BidComparisonBidApprovalsFilter filter);
        Task<List<BidComparisonApprovalInformationLoad>> GetComparisonApprovalInformations(BidComparisonApprovalInformationFilter filter);
        Task<List<BidComparisonBidDetailsLoad>> GetComparisonBidDetails(BidComparisonBidDetailsFilter filter);
        Task<List<BidComparisonRFQDetailsLoad>> GetComparisonRFQDetails(BidComparisonRFQDetailsFilter filter);
        Task<List<BidComparisonBidHeaderLoad>> GetComparisonBidHeader(BidComparisonBidHeaderFilter filter);
        Task<List<BidComparisonBidDto>> GetComparisonBidsLoad(BidComparisonBidHeaderFilter filter);
        Task<BidComparisonHeaderLoad> GetComparisonHeader(BidComparisonHeaderFilter filter);
        Task<List<BidComparisonSingleSourceReasonsLoad>> GetComparisonSingleSourceReasons(BidComparisonSingleSourceReasonsFilter filter);

        Task<List<BidComparisonWFALoad>> GetComparisonWFA(BidComparisonWFAFilter filter);
        Task<List<BidComparisonDraftLoad>> GetComparisonDraft(BidComparisonDraftFilter filter);
        Task<List<BidComparisonHeldLoad>> GetComparisonHeld(BidComparisonHeldFilter filter);
        Task<List<BidComparisonMyChartsLoad>> GetComparisonMyCharts(BidComparisonMyChartsFilter filter);
        Task<List<BidComparisonNotReleasedLoad>> GetComparisonNotReleased(BidComparisonNotReleasedFilter filter);
        Task<List<BidComparisonRejectedLoad>> GetComparisonRejected(BidComparisonRejectedFilter filter);
        Task<bool> HoldBidComparison(HoldBidComparisonRequest request);
        Task<bool> BidComparisonSummarySave(DataTable dataTable);
        Task<List<BidComparisonSummary>> BidComparisonSummaryLoad(int bidComparisonId);
    }
}
