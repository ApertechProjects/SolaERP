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
            var data = new List<BidAllDto>
            {
                new BidAllDto
                {
                    ApproveStatus = "0",
                    BidMainId = 1,
                    BidNo = "0",
                    CurrencyCode= "AZN",
                    DeliveryTime= "24 Bours",
                    DeliveryTerms = "TestDeliveryTerm",
                    ExpectedCost = 200,
                    LineNo = 1,
                    OperatorComment = "TestComment",
                    PaymentTerms= "TestPaymentTerms",
                    RFQNo= "1",
                    Status = "1",
                    VendorCode = "TestVendorCode",
                    VendorName = "TestVendorName"
                },
                new BidAllDto
                {
                    ApproveStatus = "0",
                    BidMainId = 2,
                    BidNo = "2",
                    CurrencyCode= "AZN",
                    DeliveryTime= "48 Bours",
                    DeliveryTerms = "TestDeliveryTerm2",
                    ExpectedCost = 500,
                    LineNo = 2,
                    OperatorComment = "TestComment2",
                    PaymentTerms= "TestPaymentTerms2",
                    RFQNo= "2",
                    Status = "2",
                    VendorCode = "TestVendorCode2",
                    VendorName = "TestVendorName2"
                },
            };
            return ApiResponse<List<BidAllDto>>.Success(data, 200);

            //var data = await _bidRepository.GetAllAsync(_mapper.Map<BidAllFilter>(filter));
            //var dtos = _mapper.Map<List<BidAllDto>>(data);

            //return ApiResponse<List<BidAllDto>>.Success(dtos, 200);
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
