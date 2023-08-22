using SolaERP.Application.Entities.Bid;
using SolaERP.Application.Entities.BidComparison;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IBidComparisonRepository
    {
        Task<bool> AddComparisonAsync(BidComparisonIUD entity);
        Task<bool> ApproveComparisonAsync(BidComparisonApprove entity);
        Task<bool> SendComparisonToApprove(BidComparisonSendToApprove filter);
        Task<List<BidComparisonAll>> GetComparisonAll(BidComparisonAllFilter filter);
        Task<List<BidComparisonBidApprovalsLoad>> GetComparisonBidApprovals(BidComparisonBidApprovalsFilter filter);
        Task<List<BidComparisonBidDetailsLoad>> GetComparisonBidDetails(BidComparisonBidDetailsFilter filter);
        Task<List<BidComparisonRFQDetailsLoad>> GetComparisonRFQDetails(BidComparisonRFQDetailsFilter filter);
        Task<BidComparisonBidHeaderLoad> GetComparisonBidHeader(BidComparisonBidHeaderFilter filter);
        Task<BidComparisonHeaderLoad> GetComparisonHeader(BidComparisonHeaderFilter filter);

        Task<List<BidComparisonDraftLoad>> GetComparisonDraft(BidComparisonDraftFilter filter);

    }
}
