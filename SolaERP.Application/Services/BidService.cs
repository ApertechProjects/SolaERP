using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Bid;
using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Bid;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Services
{
    public class BidService : IBidService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBidRepository _bidRepository;

        public BidService(IUnitOfWork unitOfWork, IMapper mapper, IBidRepository bidRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _bidRepository = bidRepository;
        }

        public async Task<ApiResponse<List<BidAllDto>>> GetAllAsync(BidAllFilter filter)
        {
            var data = await _bidRepository.GetAllAsync(filter);
            var dtos = _mapper.Map<List<BidAllDto>>(data);

            return ApiResponse<List<BidAllDto>>.Success(dtos, 200);
        }

        public async Task<ApiResponse<int>> SaveBidMainAsync(BidMainDto bidMain)
        {
            var entity = _mapper.Map<BidMain>(bidMain);
            return ApiResponse<int>.Success(await _bidRepository.AddMainAsync(entity), 200);
        }
    }
}
