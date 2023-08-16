using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Bid;
using SolaERP.Application.Dtos.BidComparison;
using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Bid;
using SolaERP.Application.Entities.BidComparison;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Services
{
    public class BidComparisonService : IBidComparisonService

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBidComparisonRepository _bidComparisonRepository;

        public BidComparisonService(IUnitOfWork unitOfWork, IMapper mapper, IBidComparisonRepository bidComparisonRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _bidComparisonRepository = bidComparisonRepository;
        }

        public async Task<ApiResponse<bool>> SaveBidComparisonAsync(BidComparisonCreateDto bidComparison)
        {
            var entity = _mapper.Map<BidComparisonIUD>(bidComparison);
            var saveResponse = await _bidComparisonRepository.AddComparisonAsync(entity);

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(saveResponse, 200);
        }

        public async Task<ApiResponse<bool>> ApproveBidComparisonAsync(BidComparisonApproveDto bidComparisonApprove)
        {
            var entity = _mapper.Map<BidComparisonApprove>(bidComparisonApprove);
            var saveResponse = await _bidComparisonRepository.ApproveComparisonAsync(entity);

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(saveResponse, 200);
        }

        public async Task<ApiResponse<BidComparisonDto>> GetBidComparisonAsync(BidComparisonFilterDto filter)
        {
            var comparison = new BidComparisonDto();
            var headerFilter = new BidComparisonHeaderFilter { BidComparisonId = filter.BidComparisonId, UserId = filter.UserId };
            var header = await _bidComparisonRepository.GetComparisonHeader(headerFilter);
            comparison.BidComparisonHeader = _mapper.Map<BidComparisonHeaderLoadDto>(header);

            var bidHeaderFilter = new BidComparisonBidHeaderFilter { BidComparisonId = filter.BidComparisonId, UserId = filter.UserId };
            var bids = await _bidComparisonRepository.GetComparisonBidHeader(bidHeaderFilter);
            comparison.Bids = _mapper.Map<List<BidComparisonBidHeaderLoadDto>>(bids);

            var bidDetailsFilter = new BidComparisonBidDetailsFilter { BidComparisonId = filter.BidComparisonId, UserId = filter.UserId };
            var bidDetails = await _bidComparisonRepository.GetComparisonBidDetails(bidDetailsFilter);

            var bidApprovalsFilter = new BidComparisonBidApprovalsFilter { BidComparisonId = filter.BidComparisonId, UserId = filter.UserId };
            var bidApprovals = await _bidComparisonRepository.GetComparisonBidApprovals(bidApprovalsFilter);

            foreach (var bid in comparison.Bids)
            {
                bid.Details = _mapper.Map<List<BidComparisonBidDetailsLoadDto>>(bidDetails.Where(x => x.BidMainId == bid.BidMainId));
                bid.Approvals = _mapper.Map<List<BidComparisonBidApprovalsLoadDto>>(bidApprovals.Where(x=> x.BidMainId == bid.BidMainId));
            }

            var rfqDetailsFilter = new BidComparisonRFQDetailsFilter { BidComparisonId = filter.BidComparisonId, UserId = filter.UserId };
            var rfqDetails = _bidComparisonRepository.GetComparisonRFQDetails(rfqDetailsFilter);
            comparison.RfqDetails = _mapper.Map<List<BidComparisonRFQDetailsLoadDto>>(rfqDetails);


            return ApiResponse<BidComparisonDto>.Success(comparison, 200);
        }
    }
}
