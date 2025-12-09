using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.BidComparison;
using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.BidComparison;
using SolaERP.Application.UnitOfWork;
using SolaERP.Persistence.Utils;
using SolaERP.Application.Enums;
using System.Diagnostics;
using SolaERP.Application.Dtos.Request;
using SolaERP.Application.Entities.Payment;
using SolaERP.Application.Models;

namespace SolaERP.Persistence.Services
{
    public class BidComparisonService : IBidComparisonService

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBidComparisonRepository _bidComparisonRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IApproveStageMainRepository _approveStageMainRepository;
        private readonly IRfqRepository _rfqRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IAttachmentService _attachmentService;
        private readonly IBuyerService _buyerService;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IMailService _mailService;


        public BidComparisonService(IUnitOfWork unitOfWork, IMapper mapper,
            IBidComparisonRepository bidComparisonRepository,
            IOrderRepository orderRepository,
            IApproveStageMainRepository approveStageMainRepository,
            IRfqRepository rfqRepository,
            IFileUploadService fileUploadService,
            IAttachmentService attachmentService,
            IBuyerService buyerService,
            IBackgroundTaskQueue taskQueue,
            IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _bidComparisonRepository = bidComparisonRepository;
            _approveStageMainRepository = approveStageMainRepository;
            _rfqRepository = rfqRepository;
            _fileUploadService = fileUploadService;
            _attachmentService = attachmentService;
            _buyerService = buyerService;
            _taskQueue = taskQueue;
            _mailService = mailService;

        }

        public async Task<ApiResponse<BidComparisonCreateResponseDto>> SaveBidComparisonAsync(
            BidComparisonCreateDto bidComparison)
        {
            var entity = _mapper.Map<BidComparisonIUD>(bidComparison);
            var saveResponse = await _bidComparisonRepository.AddComparisonAsync(entity);

            await _attachmentService.SaveAttachmentAsync(bidComparison.Attachments,
                SourceType.BID_COMP,
                saveResponse);

            await _unitOfWork.SaveChangesAsync();

            BidComparisonCreateResponseDto bidComparisonCreateResponseDto = new BidComparisonCreateResponseDto();
            bidComparisonCreateResponseDto.BidComparisonId = saveResponse;

            return ApiResponse<BidComparisonCreateResponseDto>.Success(bidComparisonCreateResponseDto, 200);
        }

        public async Task<ApiResponse<bool>> SaveBidsAsync(
            BidComparisonBidsCreateRequestDto comparison)
        {
            var table = comparison.Bids.ConvertListOfCLassToDataTable();
            var result = await _bidComparisonRepository.SaveComparisonBids(comparison.BidComparisonId, table);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(true, 200);
        }

        public async Task<ApiResponse<bool>> ApproveBidComparisonsAsync(
            List<BidComparisonApproveDto> bidComparisonApproves, string userIdentity)
        {
            foreach (var bidComparisonApprove in bidComparisonApproves)
            {
                var entity = _mapper.Map<BidComparisonApprove>(bidComparisonApprove);
                entity.UserId = Convert.ToInt32(userIdentity);
                await _bidComparisonRepository.ApproveComparisonAsync(entity);
            }

            await _unitOfWork.SaveChangesAsync();

            var bidMainIds = bidComparisonApproves.Select(x => x.BidMainId).Distinct().ToList();
            foreach (var item in bidMainIds)
            {
                var order = await _orderRepository.CheckOrderList(item);
                if (order)
                {
                    int i = 1;
                    var createOrder = await _bidComparisonRepository.OrderCreateFromApproveBid(new CreateOrderFromBidDto
                        { BidMainId = item, UserId = Convert.ToInt32(userIdentity) });
                    await _unitOfWork.SaveChangesAsync();
                    Debug.WriteLine(i + " time worked");
                    i++;
                }
            }

            return ApiResponse<bool>.Success(true, 200);
        }

        public async Task<ApiResponse<bool>> BidApproveAsync(
            BidComparisonBidApproveDto approve, string userIdentity)
        {
            var saveResponse = await _bidComparisonRepository.BidApprove(approve, Convert.ToInt32(userIdentity));
            await _unitOfWork.SaveChangesAsync();

            // await BidApproveAsyncForMail(approve, userIdentity);

            return ApiResponse<bool>.Success(saveResponse, 200);
        }
        
        public async Task<ApiResponse<bool>> BidApproveAsyncForMail(
            BidComparisonBidApproveDto approve, string userIdentity)
        {
            var datas = await _bidComparisonRepository.GetById(approve.BidComparisonId);

            if (datas != null && datas.Any())
            {
                var data = datas[0];
                string buyerEmail =
                    await _buyerService.FindBuyerEmailByBuyerName(data.Buyer, data.BusinessUnitId);
            
                string businessUnitName =
                    await _buyerService.FindBusinessUnitNameByBuyerName(data.Buyer, data.BusinessUnitId);
                
                RequestBuyerData buyerData = new RequestBuyerData();
                buyerData.BuyerName = data.Buyer;
                buyerData.Email = buyerEmail;
                buyerData.RequestNo = data.ComparisonNo;
                buyerData.RequestMainId = data.BidComparisonId;
                buyerData.BusinessUnitName = businessUnitName;
                buyerData.Language = "eng";
            
            
                _taskQueue.QueueBackgroundWorkItem(async token =>
                {
                    await _mailService.SendBidComparisonForBuyer(buyerData);
                });
            }

            return ApiResponse<bool>.Success(true, 200);
        }

        public async Task<ApiResponse<bool>> BidRejectAsync(
            BidComparisonBidRejectDto reject, string userIdentity)
        {
            await _bidComparisonRepository.BidReject(reject, Convert.ToInt32(userIdentity));
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.Success(true, 200);
        }

        public async Task<ApiResponse<bool>> SendComparisonToApproveAsync(
            BidComparisonSendToApproveDto bidComparisonSendToApprove)
        {
            var entity = _mapper.Map<BidComparisonSendToApprove>(bidComparisonSendToApprove);
            var saveResponse = await _bidComparisonRepository.SendComparisonToApprove(entity);

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(saveResponse, 200);
        }

        public async Task<ApiResponse<List<BidComparisonAllDto>>> GetBidComparisonAllAsync(
            BidComparisonAllFilterDto filter)
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
            header.RequiredOnSiteDate = header.RequiredOnSiteDate.ConvertDateToValidDate();
            header.SentDate = header.SentDate.ConvertDateToValidDate();
            header.RFQDate = header.RFQDate.ConvertDateToValidDate();
            header.DesiredDeliveryDate = header.DesiredDeliveryDate.ConvertDateToValidDate();
            header.ComparisonDate = header.ComparisonDate.ConvertDateToValidDate();
            header.RFQDeadline = header.RFQDeadline.ConvertDateToValidDate();
            header.Entrydate = header.Entrydate.ConvertDateToValidDate();
            header.Comparisondeadline = header.Comparisondeadline.ConvertDateToValidDate();
            header.Attachments = await _attachmentService.GetAttachmentsAsync(header.BidComparisonId,
                SourceType.BID_COMP, Modules.BidComparison);

            comparison.BidComparisonHeader = _mapper.Map<BidComparisonHeaderLoadDto>(header);

            var singleSourceFilter = new BidComparisonSingleSourceReasonsFilter { RFQMainId = filter.RFQMainId };
            var singleSourceReasons =
                await _bidComparisonRepository.GetComparisonSingleSourceReasons(singleSourceFilter);
            comparison.BidComparisonHeader.SingleSourceReasons =
                _mapper.Map<List<BidComparisonSingleSourceReasonsLoadDto>>(singleSourceReasons);

            var rfqSingleSourceReasons =
                await _rfqRepository.GetSingleSourceReasons(comparison.BidComparisonHeader.RFQMainId);
            comparison.BidComparisonHeader.RFQSingleSourceReasons =
                _mapper.Map<List<RFQSingleSourceReasonsLoadDto>>(rfqSingleSourceReasons);

            var bidHeaderFilter = new BidComparisonBidHeaderFilter
                { RFQMainId = filter.RFQMainId, UserId = filter.UserId };
            var bids = await _bidComparisonRepository.GetComparisonBidHeader(bidHeaderFilter);
            comparison.Bids = _mapper.Map<List<BidComparisonBidHeaderLoadDto>>(bids);

            var bidDetailsFilter = new BidComparisonBidDetailsFilter
                { RFQMainId = filter.RFQMainId, UserId = filter.UserId };
            var bidDetails = await _bidComparisonRepository.GetComparisonBidDetails(bidDetailsFilter);

            var bidApprovalsFilter = new BidComparisonBidApprovalsFilter
                { RFQMainId = filter.RFQMainId, UserId = filter.UserId };
            var bidApprovals = await _bidComparisonRepository.GetComparisonBidApprovals(bidApprovalsFilter);

            foreach (var bid in comparison.Bids)
            {
                bid.BidDetails =
                    _mapper.Map<List<BidComparisonBidDetailsLoadDto>>(
                        bidDetails.Where(x => x.BidMainId == bid.BidMainId));
                bid.BidApprovals =
                    _mapper.Map<List<BidComparisonBidApprovalsLoadDto>>(bidApprovals.Where(x =>
                        x.BidMainId == bid.BidMainId));
            }

            var rfqDetailsFilter = new BidComparisonRFQDetailsFilter
                { RFQMainId = filter.RFQMainId, UserId = filter.UserId };
            var rfqDetails = await _bidComparisonRepository.GetComparisonRFQDetails(rfqDetailsFilter);
            comparison.RfqDetails = _mapper.Map<List<BidComparisonRFQDetailsLoadDto>>(rfqDetails);

            var approvalInformationFilter = new BidComparisonApprovalInformationFilter { RFQMainId = filter.RFQMainId };
            var approvalInformations =
                await _bidComparisonRepository.GetComparisonApprovalInformations(approvalInformationFilter);
            foreach (var item in approvalInformations)
            {
                item.SignaturePhoto = _fileUploadService.GetFileLink(item.SignaturePhoto, Modules.Users);
                item.UserPhoto = _fileUploadService.GetFileLink(item.UserPhoto, Modules.Users);
            }


            comparison.ApprovalInformations =
                _mapper.Map<List<BidComparisonApprovalInformationLoadDto>>(approvalInformations);
            foreach (var item in comparison.ApprovalInformations)
                item.ApproveDate = item.ApproveDate.ConvertDateToValidDate();

            return ApiResponse<BidComparisonDto>.Success(comparison, 200);
        }

        public async Task<ApiResponse<BidComparisonLoadDto>> GetBidComparisonLoadAsync(BidComparisonFilterDto filter)
        {
            var comparison = new BidComparisonLoadDto();
            var headerFilter = new BidComparisonHeaderFilter
                { RFQMainId = filter.RFQMainId, UserId = filter.UserId, BidComparisonId = filter.BidComparisonId };
            var header = await _bidComparisonRepository.GetComparisonHeader(headerFilter);
            header.RequiredOnSiteDate = header.RequiredOnSiteDate.ConvertDateToValidDate();
            header.SentDate = header.SentDate.ConvertDateToValidDate();
            header.RFQDate = header.RFQDate.ConvertDateToValidDate();
            header.DesiredDeliveryDate = header.DesiredDeliveryDate.ConvertDateToValidDate();
            header.ComparisonDate = header.ComparisonDate.ConvertDateToValidDate();
            header.RFQDeadline = header.RFQDeadline.ConvertDateToValidDate();
            header.Entrydate = header.Entrydate.ConvertDateToValidDate();
            header.Comparisondeadline = header.Comparisondeadline.ConvertDateToValidDate();
            header.Attachments = await _attachmentService.GetAttachmentsAsync(header.BidComparisonId,
                SourceType.BID_COMP, Modules.BidComparison);

            comparison.BidComparisonHeader = _mapper.Map<BidComparisonHeaderLoadDto>(header);

            var singleSourceFilter = new BidComparisonSingleSourceReasonsFilter { RFQMainId = filter.RFQMainId };
            var singleSourceReasons =
                await _bidComparisonRepository.GetComparisonSingleSourceReasons(singleSourceFilter);
            comparison.BidComparisonHeader.SingleSourceReasons =
                _mapper.Map<List<BidComparisonSingleSourceReasonsLoadDto>>(singleSourceReasons);

            var rfqSingleSourceReasons =
                await _rfqRepository.GetSingleSourceReasons(comparison.BidComparisonHeader.RFQMainId);
            comparison.BidComparisonHeader.RFQSingleSourceReasons =
                _mapper.Map<List<RFQSingleSourceReasonsLoadDto>>(rfqSingleSourceReasons);

            //Bids
            var bidHeaderFilter = new BidComparisonBidHeaderFilter
                { RFQMainId = filter.RFQMainId, BidComparisonId = filter.BidComparisonId, UserId = filter.UserId };
            comparison.Bids = await _bidComparisonRepository.GetComparisonBidsLoad(bidHeaderFilter);

            //Approval infos
            var approvalInformationFilter = new BidComparisonApprovalInformationFilter { RFQMainId = filter.RFQMainId };
            var approvalInformations =
                await _bidComparisonRepository.GetComparisonApprovalInformations(approvalInformationFilter);
            foreach (var item in approvalInformations)
            {
                item.SignaturePhoto = _fileUploadService.GetFileLink(item.SignaturePhoto, Modules.Users);
                item.UserPhoto = _fileUploadService.GetFileLink(item.UserPhoto, Modules.Users);
            }

            comparison.ApprovalInformations =
                _mapper.Map<List<BidComparisonApprovalInformationLoadDto>>(approvalInformations);
            foreach (var item in comparison.ApprovalInformations)
                item.ApproveDate = item.ApproveDate.ConvertDateToValidDate();

            return ApiResponse<BidComparisonLoadDto>.Success(comparison, 200);
        }

        public async Task<ApiResponse<List<BidComparisonDraftLoadDto>>> GetComparisonDraft(
            BidComparisonDraftFilterDto filterDto)
        {
            var filter = _mapper.Map<BidComparisonDraftFilter>(filterDto);
            var data = await _bidComparisonRepository.GetComparisonDraft(filter);
            var dtos = _mapper.Map<List<BidComparisonDraftLoadDto>>(data);
            return ApiResponse<List<BidComparisonDraftLoadDto>>.Success(dtos, 200);
        }

        public async Task<ApiResponse<List<BidComparisonWFALoadDto>>> GetComparisonWFA(
            BidComparisonWFAFilterDto filterDto)
        {
            var filter = _mapper.Map<BidComparisonWFAFilter>(filterDto);
            var data = await _bidComparisonRepository.GetComparisonWFA(filter);
            var dtos = _mapper.Map<List<BidComparisonWFALoadDto>>(data);
            return ApiResponse<List<BidComparisonWFALoadDto>>.Success(dtos, 200);
        }

        public async Task<ApiResponse<List<BidComparisonHeldLoadDto>>> GetComparisonHeld(
            BidComparisonHeldFilterDto filterDto)
        {
            var filter = _mapper.Map<BidComparisonHeldFilter>(filterDto);
            var data = await _bidComparisonRepository.GetComparisonHeld(filter);
            var dtos = _mapper.Map<List<BidComparisonHeldLoadDto>>(data);
            return ApiResponse<List<BidComparisonHeldLoadDto>>.Success(dtos, 200);
        }

        public async Task<ApiResponse<List<BidComparisonMyChartsLoadDto>>> GetComparisonMyCharts(
            BidComparisonMyChartsFilterDto filterDto, string userIdentity)
        {
            var filter = _mapper.Map<BidComparisonMyChartsFilter>(filterDto);
            filter.UserId = Convert.ToInt32(userIdentity);
            var data = await _bidComparisonRepository.GetComparisonMyCharts(filter);
            var dtos = _mapper.Map<List<BidComparisonMyChartsLoadDto>>(data);
            return ApiResponse<List<BidComparisonMyChartsLoadDto>>.Success(dtos, 200);
        }

        public async Task<ApiResponse<List<BidComparisonNotReleasedLoadDto>>> GetComparisonNotReleased(
            BidComparisonNotReleasedFilterDto filterDto)
        {
            var filter = _mapper.Map<BidComparisonNotReleasedFilter>(filterDto);
            var data = await _bidComparisonRepository.GetComparisonNotReleased(filter);
            var dtos = _mapper.Map<List<BidComparisonNotReleasedLoadDto>>(data);
            return ApiResponse<List<BidComparisonNotReleasedLoadDto>>.Success(dtos, 200);
        }

        public async Task<ApiResponse<List<BidComparisonRejectedLoadDto>>> GetComparisonRejected(
            BidComparisonRejectedFilterDto filterDto)
        {
            var filter = _mapper.Map<BidComparisonRejectedFilter>(filterDto);
            var data = await _bidComparisonRepository.GetComparisonRejected(filter);
            var dtos = _mapper.Map<List<BidComparisonRejectedLoadDto>>(data);
            return ApiResponse<List<BidComparisonRejectedLoadDto>>.Success(dtos, 200);
        }

        public async Task<ApiResponse<bool>> HoldBidComparison(HoldBidComparisonRequest request)
        {
            return ApiResponse<bool>.Success(
                await _bidComparisonRepository.HoldBidComparison(request)
            );
        }

        public async Task<bool> OrderCreateFromApproveBid(CreateOrderFromBidDto entity)
        {
            var data = await _bidComparisonRepository.OrderCreateFromApproveBid(entity);
            return data;
        }

        public async Task<ApiResponse<bool>> BidComparisonSummarySave(List<BidComparisonSummaryDto> summaryDto)
        {
            var datatable = summaryDto.ConvertListOfCLassToDataTable();
            var data = await _bidComparisonRepository.BidComparisonSummarySave(datatable);
            await _unitOfWork.SaveChangesAsync();
            if (data)
                return ApiResponse<bool>.Success(200);
            return ApiResponse<bool>.Fail("Save return error", 500);
        }

        public async Task<ApiResponse<List<BidComparisonSummaryDto>>> BidComparisonSummaryLoad(int bidComparisonId)
        {
            var data = await _bidComparisonRepository.BidComparisonSummaryLoad(bidComparisonId);
            var map = _mapper.Map<List<BidComparisonSummaryDto>>(data);
            return ApiResponse<List<BidComparisonSummaryDto>>.Success(map);
        }

        public async Task<ApiResponse<List<BidComparisonApprovalInfoDto>>> BidComparisonApprovalInfo(
            int bidComparisonId)
        {
            var data = await _bidComparisonRepository.BidComparisonApprovalInfo(bidComparisonId);
            var map = _mapper.Map<List<BidComparisonApprovalInfoDto>>(data);
            return ApiResponse<List<BidComparisonApprovalInfoDto>>.Success(map);
        }

        public async Task<ApiResponse<bool>> Retrieve(int bidComparisonId, int userId)
        {
            var result = await _bidComparisonRepository.Retrieve(bidComparisonId, userId);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(result);
        }

        public async Task<ApiResponse<bool>> Delete(int bidComparisonId)
        {
            var result = await _bidComparisonRepository.Delete(bidComparisonId);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(result);
        }
        
        public async Task<ApiResponse<List<BidMainListByRfqMainDto>>> GetBidListByRfqMainId(int rfqMainId)
        {
            var data = await _bidComparisonRepository.GetBidListByRfqMainId(rfqMainId);
            var dtos = _mapper.Map<List<BidMainListByRfqMainDto>>(data);
            return ApiResponse<List<BidMainListByRfqMainDto>>.Success(dtos, 200);
        }
    }
}