using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Bid;
using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Bid;
using SolaERP.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolaERP.Application.Enums;

namespace SolaERP.Persistence.Services
{
    public class BidService : IBidService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBidRepository _bidRepository;
        private readonly ISupplierEvaluationRepository _supplierEvaluationRepository;
        private readonly IRfqRepository _rfqRepository;
        private readonly IAttachmentService _attachmentService;

        public BidService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IBidRepository bidRepository,
            ISupplierEvaluationRepository supplierEvaluationRepository,
            IRfqRepository rfqRepository,
            IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _bidRepository = bidRepository;
            _supplierEvaluationRepository = supplierEvaluationRepository;
            _rfqRepository = rfqRepository;
            _attachmentService = attachmentService;
        }

        public async Task<ApiResponse<List<BidAllDto>>> GetAllAsync(BidAllFilterDto filter)
        {
            var data = await _bidRepository.GetAllAsync(_mapper.Map<BidAllFilter>(filter));
            var dtos = _mapper.Map<List<BidAllDto>>(data);

            return ApiResponse<List<BidAllDto>>.Success(dtos, 200);
        }

        public async Task<ApiResponse<List<BidDetailsLoadDto>>> GetBidDetailsAsync(BidDetailsFilterDto filter)
        {
            var data = await _bidRepository.GetBidDetailsAsync(_mapper.Map<BidDetailsFilter>(filter));
            var dtos = _mapper.Map<List<BidDetailsLoadDto>>(data);

            return ApiResponse<List<BidDetailsLoadDto>>.Success(dtos, 200);
        }

        public async Task<ApiResponse<BidMainLoadDto>> GetMainLoadAsync(int bidMainId)
        {
            var bidMain = await _bidRepository.GetMainLoadAsync(bidMainId);
            var model = _mapper.Map<BidMainLoadDto>(bidMain);

            var details = await _bidRepository.GetBidDetailsAsync(new BidDetailsFilter { BidMainId = bidMainId });
            var dtos = _mapper.Map<List<BidDetailsLoadDto>>(details);
            foreach (var item in dtos)
            {
                item.Attachments = await _attachmentService.GetAttachmentsAsync(item.BidDetailId, SourceType.BID, Modules.Bid);
            }
            model.Details = dtos;
            model.RFQMain = _mapper.Map<RFQMainDto>(await _rfqRepository.GetRFQMainAsync(model.RFQMainId));
            model.Attachments = await _attachmentService.GetAttachmentsAsync(bidMainId, SourceType.BID, Modules.Bid);
            model.CommercialAttachments =
                await _attachmentService.GetAttachmentsAsync(bidMainId, SourceType.BID_COMM, Modules.Bid);

            return ApiResponse<BidMainLoadDto>.Success(model, 200);
        }

        public async Task<ApiResponse<BidCardDto>> GetBidCardAsync(int bidMainId)
        {
            var card = new BidCardDto();
            var bidMain = await _bidRepository.GetMainLoadAsync(bidMainId);
            var model = _mapper.Map<BidMainLoadDto>(bidMain);

            var details = await _bidRepository.GetBidDetailsAsync(new BidDetailsFilter { BidMainId = bidMainId });
            var dtos = _mapper.Map<List<BidDetailsLoadDto>>(details);
            model.Details = dtos;

            card.BidMain = model;
            card.DeliveryTermsList = await _supplierEvaluationRepository.GetDeliveryTermsAsync();
            card.PaymentTermsList = await _supplierEvaluationRepository.GetPaymentTermsAsync();

            return ApiResponse<BidCardDto>.Success(card, 200);
        }

        public async Task<ApiResponse<BidIUDResponse>> SaveBidMainAsync(BidMainDto bidMain, string userIdentity)
        {
            var entity = _mapper.Map<BidMain>(bidMain);
            entity.UserId = Convert.ToInt32(userIdentity);

            var details = _mapper.Map<List<BidDetail>>(bidMain.BidDetails);
            var saveResponse = await _bidRepository.BidMainIUDAsync(entity);

            await _attachmentService.SaveAttachmentAsync(bidMain.Attachments, SourceType.BID, saveResponse.Id);
            await _attachmentService.SaveAttachmentAsync(bidMain.CommercialAttachments, SourceType.BID_COMM, saveResponse.Id);

            foreach (var detail in details)
                detail.BidMainId = saveResponse.Id;

            await _bidRepository.SaveBidDetailsAsync(details);

            var detailIds = await _bidRepository.GetDetailIds(saveResponse.Id);
            for (int i = 0; i < bidMain.BidDetails.Count; i++)
            {
                BidDetailDto item = bidMain.BidDetails[i];
                item.BidDetailId = detailIds[i];
                await _attachmentService.SaveAttachmentAsync(item.Attachments, SourceType.BID, item.BidDetailId);
            }

            saveResponse.BidDetailIds = detailIds;
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<BidIUDResponse>.Success(saveResponse, 200);
        }

        public async Task<ApiResponse<bool>> BidDisqualifyAsync(BidDisqualifyDto dto, string userIdentity)
        {
            var entity = _mapper.Map<BidDisqualify>(dto);
            entity.UserId = Convert.ToInt32(userIdentity);
            var saveResponse = await _bidRepository.BidDisqualifyAsync(entity);

            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.Success(saveResponse, 200);
        }

        public async Task<ApiResponse<bool>> DeleteBidMainAsync(int bidMainId, string userIdentity)
        {
            int userId = Convert.ToInt32(userIdentity);
            var saveResponse =
                await _bidRepository.BidMainIUDAsync(new BidMain { BidMainId = bidMainId, UserId = userId });

            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.Success(true, 200);
        }

        public async Task<ApiResponse<List<BidRFQListLoadDto>>> GetRfqListAsync(string userIdentity, int businessUnitId)
        {
            var filter = new BidRFQListFilter { UserId = Convert.ToInt32(userIdentity), BusinessUnitId = businessUnitId };
            var data = await _bidRepository.GetRFQListForBidAsync(filter);
            var dtos = _mapper.Map<List<BidRFQListLoadDto>>(data);

            return ApiResponse<List<BidRFQListLoadDto>>.Success(dtos, 200);
        }

        public async Task<ApiResponse<bool>> OrderCreateFromApproveBidsAsync(List<int> bidMainIdList,
            string userIdentity)
        {
            int userId = Convert.ToInt32(userIdentity);
            foreach (var bidMainId in bidMainIdList)
            {
                await _bidRepository.OrderCreateFromApproveBidAsync(bidMainId, userId);
            }

            return ApiResponse<bool>.Success(true);
        }
    }
}