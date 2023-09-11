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

        public async Task<ApiResponse<bool>> SendComparisonToApproveAsync(BidComparisonSendToApproveDto bidComparisonSendToApprove)
        {
            var entity = _mapper.Map<BidComparisonSendToApprove>(bidComparisonSendToApprove);
            var saveResponse = await _bidComparisonRepository.SendComparisonToApprove(entity);

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(saveResponse, 200);
        }

        public async Task<ApiResponse<List<BidComparisonAllDto>>> GetBidComparisonAllAsync(BidComparisonAllFilterDto filter)
        {
            var data = await _bidComparisonRepository.GetComparisonAll(_mapper.Map<BidComparisonAllFilter>(filter));
            var dtos = _mapper.Map<List<BidComparisonAllDto>>(data);

            return ApiResponse<List<BidComparisonAllDto>>.Success(dtos, 200);
        }

        public async Task<ApiResponse<BidComparisonDto>> GetBidComparisonAsync(BidComparisonFilterDto filter)
        {
            var comparison = new BidComparisonDto();
            var headerFilter = new BidComparisonHeaderFilter { RFQMainId = filter.RFQMainId, UserId = filter.UserId };
            var header = await _bidComparisonRepository.GetComparisonHeader(headerFilter);
            comparison.BidComparisonHeader = _mapper.Map<BidComparisonHeaderLoadDto>(header);

            var singleSourceFilter = new BidComparisonSingleSourceReasonsFilter { RFQMainId = filter.RFQMainId };
            var singleSourceReasons = await _bidComparisonRepository.GetComparisonSingleSourceReasons(singleSourceFilter);
            comparison.BidComparisonHeader.SingleSourceReasons = _mapper.Map<List<BidComparisonSingleSourceReasonsLoadDto>>(singleSourceReasons);

            var bidHeaderFilter = new BidComparisonBidHeaderFilter { RFQMainId = filter.RFQMainId, UserId = filter.UserId };
            var bids = await _bidComparisonRepository.GetComparisonBidHeader(bidHeaderFilter);
            comparison.Bids = _mapper.Map<List<BidComparisonBidHeaderLoadDto>>(bids);

            var bidDetailsFilter = new BidComparisonBidDetailsFilter { RFQMainId = filter.RFQMainId, UserId = filter.UserId };
            var bidDetails = await _bidComparisonRepository.GetComparisonBidDetails(bidDetailsFilter);

            var bidApprovalsFilter = new BidComparisonBidApprovalsFilter { RFQMainId = filter.RFQMainId, UserId = filter.UserId };
            var bidApprovals = await _bidComparisonRepository.GetComparisonBidApprovals(bidApprovalsFilter);

            foreach (var bid in comparison.Bids)
            {
                bid.BidDetails = _mapper.Map<List<BidComparisonBidDetailsLoadDto>>(bidDetails.Where(x => x.BidMainId == bid.BidMainId));
                bid.BidApprovals = _mapper.Map<List<BidComparisonBidApprovalsLoadDto>>(bidApprovals.Where(x=> x.BidMainId == bid.BidMainId));
            }

            var rfqDetailsFilter = new BidComparisonRFQDetailsFilter { RFQMainId = filter.RFQMainId, UserId = filter.UserId };
            var rfqDetails = await _bidComparisonRepository.GetComparisonRFQDetails(rfqDetailsFilter);
            comparison.RfqDetails = _mapper.Map<List<BidComparisonRFQDetailsLoadDto>>(rfqDetails);

            var approvalInformationFilter = new BidComparisonApprovalInformationFilter { RFQMainId = filter.RFQMainId };
            var approvalInformations = await _bidComparisonRepository.GetComparisonApprovalInformations(approvalInformationFilter);
            comparison.ApprovalInformations = _mapper.Map<List<BidComparisonApprovalInformationLoadDto>>(approvalInformations);

            return ApiResponse<BidComparisonDto>.Success(comparison, 200);
        }

        public async Task<ApiResponse<List<BidComparisonDraftLoadDto>>> GetComparisonDraft(BidComparisonDraftFilterDto filterDto)
        {
            var filter = _mapper.Map<BidComparisonDraftFilter>(filterDto);
            var data = await _bidComparisonRepository.GetComparisonDraft(filter);
            var dtos = _mapper.Map<List<BidComparisonDraftLoadDto>>(data);
            return ApiResponse<List<BidComparisonDraftLoadDto>>.Success(dtos, 200);

        }

        public async Task<ApiResponse<List<BidComparisonWFALoadDto>>> GetComparisonWFA(BidComparisonWFAFilterDto filterDto)
        {
            var filter = _mapper.Map<BidComparisonWFAFilter>(filterDto);
            var data = await _bidComparisonRepository.GetComparisonWFA(filter);
            var dtos = _mapper.Map<List<BidComparisonWFALoadDto>>(data);
            return ApiResponse<List<BidComparisonWFALoadDto>>.Success(dtos, 200);

        }

        public async Task<ApiResponse<List<BidComparisonHeldLoadDto>>> GetComparisonHeld(BidComparisonHeldFilterDto filterDto)
        {
            var filter = _mapper.Map<BidComparisonHeldFilter>(filterDto);
            var data = await _bidComparisonRepository.GetComparisonHeld(filter);
            var dtos = _mapper.Map<List<BidComparisonHeldLoadDto>>(data);
            return ApiResponse<List<BidComparisonHeldLoadDto>>.Success(dtos, 200);
        }

        public async Task<ApiResponse<List<BidComparisonMyChartsLoadDto>>> GetComparisonMyCharts(BidComparisonMyChartsFilterDto filterDto, string userIdentity)
        {
            var filter = _mapper.Map<BidComparisonMyChartsFilter>(filterDto);
            filter.UserId = Convert.ToInt32(userIdentity);
            var data = await _bidComparisonRepository.GetComparisonMyCharts(filter);
            var dtos = _mapper.Map<List<BidComparisonMyChartsLoadDto>>(data);
            return ApiResponse<List<BidComparisonMyChartsLoadDto>>.Success(dtos, 200);
        }

        public async Task<ApiResponse<List<BidComparisonNotReleasedLoadDto>>> GetComparisonNotReleased(BidComparisonNotReleasedFilterDto filterDto)
        {
            var filter = _mapper.Map<BidComparisonNotReleasedFilter>(filterDto);
            var data = await _bidComparisonRepository.GetComparisonNotReleased(filter);
            var dtos = _mapper.Map<List<BidComparisonNotReleasedLoadDto>>(data);
            return ApiResponse<List<BidComparisonNotReleasedLoadDto>>.Success(dtos, 200);
        }

        public async Task<ApiResponse<List<BidComparisonRejectedLoadDto>>> GetComparisonRejected(BidComparisonRejectedFilterDto filterDto)
        {
            var filter = _mapper.Map<BidComparisonRejectedFilter>(filterDto);
            var data = await _bidComparisonRepository.GetComparisonRejected(filter);
            var dtos = _mapper.Map<List<BidComparisonRejectedLoadDto>>(data);
            return ApiResponse<List<BidComparisonRejectedLoadDto>>.Success(dtos, 200);
        }
    }
}
