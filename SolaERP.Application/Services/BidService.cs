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
    public class BidComparisonService : IBidComparionService

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

    }
}
