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

        public async Task<ApiResponse<List<BidAllDto>>> GetAllAsync(BidAllFilterDto filter)
        {
            var data = await _bidRepository.GetAllAsync(_mapper.Map<BidAllFilter>(filter));
            var dtos = _mapper.Map<List<BidAllDto>>(data);

            return ApiResponse<List<BidAllDto>>.Success(dtos, 200);
        }

        public async Task<ApiResponse<BidMainLoadDto>> GetMainLoadAsync(int bidMainId)
        {
            var bidMain = await _bidRepository.GetMainLoadAsync(bidMainId);
            var model = _mapper.Map<BidMainLoadDto>(bidMain);
            return ApiResponse<BidMainLoadDto>.Success(model, 200);
        }

        public async Task<ApiResponse<BidIUDResponse>> SaveBidMainAsync(BidMainDto bidMain)
        {
            var entity = _mapper.Map<BidMain>(bidMain);
            var details = _mapper.Map<List<BidDetail>>(bidMain.BidDetails);
            var saveResponse = await _bidRepository.AddMainAsync(entity);

            foreach (var detail in details)
                detail.BidMainId = saveResponse.Id;

            await _bidRepository.SaveBidDetailsAsync(details);

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<BidIUDResponse>.Success(saveResponse, 200);
        }

    }
}
