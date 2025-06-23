using System.Data.Common;
using AutoMapper;
using SolaERP.Application.Constants;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using Microsoft.Extensions.Logging;
using SolaERP.Application.Enums;
using SolaERP.Application.Dtos.Bid;
using SolaERP.Job;

namespace SolaERP.Persistence.Services
{
    public class RfqService : IRfqService
    {
        private readonly IRfqRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBusinessUnitRepository _bURepository;
        private readonly ISupplierEvaluationRepository _evaluationRepository;
        private readonly IGeneralRepository _generalRepository;
        private readonly IAttachmentService _attachmentService;
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        private readonly IVendorRepository _vendorRepository;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<RfqService> _logger;
        private readonly IBuyerService _buyerService;

        public RfqService(IUnitOfWork unitOfWork,
            IRfqRepository repository,
            IMapper mapper,
            ISupplierEvaluationRepository evaluationRepository,
            IBusinessUnitRepository bURepository,
            IGeneralRepository generalRepository,
            IAttachmentService attachmentService,
            IUserRepository userRepository,
            IMailService mailService,
            IVendorRepository vendorRepository,
            IBackgroundTaskQueue taskQueue,
            ILogger<RfqService> logger, IBuyerService buyerService)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
            _evaluationRepository = evaluationRepository;
            _bURepository = bURepository;
            _generalRepository = generalRepository;
            _attachmentService = attachmentService;
            _userRepository = userRepository;
            _mailService = mailService;
            _vendorRepository = vendorRepository;
            _taskQueue = taskQueue;
            _logger = logger;
            _buyerService = buyerService;
        }


        public async Task<ApiResponse<RfqSaveCommandResponse>> SaveRfqAsync(RfqSaveCommandRequest request,
            string useridentity)
        {
            request.UserId = Convert.ToInt32(useridentity);
            RfqSaveCommandResponse response = null;

            if (request.Id <= 0)
                response = await _repository.AddMainAsync(request);
            else
                response = await _repository.UpdateMainAsync(request);

            if (request.Attachments != null)
                await _attachmentService.SaveAttachmentAsync(request.Attachments, SourceType.RFQ, response.Id);

            CombineWithDeletedDetails(request);
            var combinedRequestDetails = CombineDeletedRequestsWithAll(request.Details);


            for (int i = 0; i < request.Details.Count; i++)
            {
                if (request.Details[i].Id < 0)
                    request.Details[i].Id = 0;
            }

            if (request.Details is not null && request.Details.Count > 0)
            {
                var res = await _repository.DetailsIUDAsync(request.Details, response.Id);
                var detailIds = await _repository.GetDetailIds(response.Id);

                for (int i = 0; i < request.Details.Count; i++)
                {
                    RfqDetailSaveModel item = request.Details[i];

                    if (detailIds.Count <= i)
                    {
                        break;
                    }

                    item.Id = detailIds[i];
                    if (item.Attachments != null)
                        await _attachmentService.SaveAttachmentAsync(item.Attachments, SourceType.RFQD, item.Id);

                    response.DetailIds = detailIds;
                }
            }

            if (combinedRequestDetails is not null && combinedRequestDetails.Count > 0)
                await _repository.RFQRequestDetailsIUDAsync(combinedRequestDetails);

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<RfqSaveCommandResponse>.Success(response, 200);
        }

        private List<RfqDetailSaveModel> GenerateModelBasedPostedDeletedIds(List<int> postedDeletedIds)
            => postedDeletedIds?.Select(x => new RfqDetailSaveModel
            {
                Id = x
            }).ToList();

        private List<RfqRequestDetailSaveModel> CombineDeletedRequestsWithAll(List<RfqDetailSaveModel> postedRfqdetails)
        {
            var combinedRequestDetails = GenerateModelBasedPostedDeletedIds(postedRfqdetails);

            for (int i = 0; i < postedRfqdetails?.Count; i++)
            {
                if (postedRfqdetails[i].RequestDetails is null)
                    continue;

                combinedRequestDetails?.AddRange(postedRfqdetails[i]?.RequestDetails);
            }

            return combinedRequestDetails;
        }


        private List<RfqRequestDetailSaveModel> GenerateModelBasedPostedDeletedIds(
            List<RfqDetailSaveModel> postedRFQDetails)
        {
            List<RfqRequestDetailSaveModel> deletedRequestDetails = new();

            for (int i = 0; i < postedRFQDetails.Count; i++)
            {
                if (postedRFQDetails[i].DeletedRequestDetailIds is null)
                    continue;

                deletedRequestDetails.AddRange(postedRFQDetails[i].DeletedRequestDetailIds.Select(
                    deletedRequestDetailId => new RfqRequestDetailSaveModel
                    {
                        Id = deletedRequestDetailId
                    }));
            }

            return deletedRequestDetails;
        }


        private void CombineWithDeletedDetails(RfqSaveCommandRequest postedModel)
        {
            var deletedRFQDetails = GenerateModelBasedPostedDeletedIds(postedModel?.DeletedDetailIds);
            if (deletedRFQDetails is null || deletedRFQDetails?.Count == 0) return;

            postedModel?.Details?.AddRange(deletedRFQDetails);
        }


        private async Task<List<RfqDetailSaveModel>> GetNotIncludedRfqDetailsOnlyWithIdsAsync(
            List<RfqDetailSaveModel> postedRfqDetals, int rfqMainId)
        {
            //Returns Ids only for iteracting with repsotiory

            var currentRFQDetailsIds = (await _repository.GetRFQDetailsAsync(rfqMainId)).Select(x => x.Id).ToList();

            var postedRfqDetailIds = postedRfqDetals.Select(x => x.Id).ToList();
            var notIncludedDetails = currentRFQDetailsIds.Except(postedRfqDetailIds).Select(x => new RfqDetailSaveModel
            {
                Id = x
            }).ToList();

            return notIncludedDetails;
        }

        //private Task<List<RfqRequestDetailSaveModel>> GetRFQRequestDetailsNotIncludedOnlyWithIds(List<RfqRequestDetailSaveModel> rfqRequestDetails, List<RfqDetailSaveModel> notIncludedDetails, int mainId)
        //{
        //    rfqRequestDetails.Except()
        //}

        //private List<RfqDetailSaveModel> GetModifiedRFQDetailsFromPostedData(List<RfqDetailSaveModel> postedRfqDetails)
        //   => postedRfqDetails.Where(x => x.ObjectStatus == Application.Enums.ObjectStatus.Modified
        //                                                || x.ObjectStatus == Application.Enums.ObjectStatus.Added).ToList();


        public async Task<ApiResponse<List<RfqAllDto>>> GetAllAsync(RfqAllFilter filter)
        {
            var rfqAlls = await _repository.GetAllAsync(filter);
            var dto = _mapper.Map<List<RfqAllDto>>(rfqAlls);

            return ApiResponse<List<RfqAllDto>>.Success(dto, 200);
        }


        public async Task<ApiResponse<List<BusinessCategory>>> GetBuCategoriesAsync()
        {
            var data = await _generalRepository.BusinessCategories();
            return ApiResponse<List<BusinessCategory>>.Success(data, 200);
        }

        public async Task<ApiResponse<List<RfqDraftDto>>> GetDraftsAsync(RfqFilter filter, string userName)
        {
            int userId = await _userRepository.ConvertIdentity(userName);
            var rfqDrafts = await _repository.GetDraftsAsync(filter, userId);
            var dto = _mapper.Map<List<RfqDraftDto>>(rfqDrafts);

            return ApiResponse<List<RfqDraftDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<RequestRfqDto>>> GetRequestsForRFQ(string userIdentity,
            RFQRequestModel model)
        {
            model.UserId = Convert.ToInt32(userIdentity);
            var rfqRequests = await _repository.GetRequestsForRfq(model);
            var dto = _mapper.Map<List<RequestRfqDto>>(rfqRequests);
            return ApiResponse<List<RequestRfqDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<SingleSourceReasonModel>>> GetSingleSourceReasonsAsync()
            => ApiResponse<List<SingleSourceReasonModel>>.Success(await _repository.GetSingleSourceReasonsAsync(), 200);

        public async Task<ApiResponse<List<RfqVendorToSend>>> GetRFQVendorsAsync(int buCategoryId)
        {
            var rfqVendors = await _repository.GetVendorsForRfqAync(buCategoryId);
            return ApiResponse<List<RfqVendorToSend>>.Success(rfqVendors, 200);
        }

        public async Task<ApiResponse<bool>> ChangeRFQStatusAsync(RfqChangeStatusModel model, string userIdentity)
        {
            var result = await _repository.ChangeRFQStatusAsync(model, Convert.ToInt32(userIdentity));
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(result, 200);
        }

        public async Task<ApiResponse<RFQMainDto>> GetRFQAsync(string userIdentity, int rfqMainId)
        {
            var mainRFQ = await _repository.GetRFQMainAsync(rfqMainId);
            if (mainRFQ is null)
                return ApiResponse<RFQMainDto>.Fail(ResultMessageConstants.ResourceNotFound, 404);

            var businessCategoriesTask = _generalRepository.BusinessCategories();
            var mainDetailsTask = _repository.GetRFQDetailsAsync(rfqMainId);
            var rfqRequestDetailsTask = _repository.GetRFQLineDeatilsAsync(rfqMainId);
            var rfqSingleReasonsTask = _repository.GetRFQSingeSourceReasons(rfqMainId);
            var businessUnitTask = _bURepository.GetBusinessUnitListByUserId(Convert.ToInt32(userIdentity));

            await Task.WhenAll(mainDetailsTask, rfqRequestDetailsTask, rfqSingleReasonsTask, businessCategoriesTask);

            var mainDetails = mainDetailsTask.Result;
            var rfqRequestDetails = rfqRequestDetailsTask.Result;
            var rfqSingleReasons = rfqSingleReasonsTask.Result;
            var matchedBuCategory =
                businessCategoriesTask.Result.FirstOrDefault(x => x.Id == mainRFQ.BusinessCategoryId);
            var matchedBU = businessUnitTask.Result
                .FirstOrDefault(x => x.BusinessUnitId == mainRFQ.BusinessUnitId);

            var mainRFQDto = _mapper.Map<RFQMainDto>(mainRFQ);
            var mainDetailsDto = _mapper.Map<List<RFQDetailDto>>(mainDetails);
            var rfqRequestDetailsDto = _mapper.Map<List<RFQRequestDetailDto>>(rfqRequestDetails);

            mainRFQDto.SingleSourceReasons = rfqSingleReasons;
            mainRFQDto.BusinessCategory = matchedBuCategory;
            mainRFQDto.BusinessUnit = matchedBU;
            mainRFQDto.Attachments =
                await _attachmentService.GetAttachmentsAsync(rfqMainId, SourceType.RFQ, Modules.Rfqs);

            var groupedRequestDetails = rfqRequestDetailsDto.GroupBy(requestLine => requestLine.GUID);
            mainDetailsDto.ForEach(detail =>
            {
                var guid = detail.GUID;
                var requestLines = groupedRequestDetails.FirstOrDefault(group => group.Key == guid)?.ToList();
                if (requestLines != null)
                {
                    detail.RequestDetails.AddRange(requestLines);
                }
            });

            mainRFQDto.Details = mainDetailsDto;
            foreach (var item in mainRFQDto.Details)
            {
                item.Attachments = await _attachmentService.GetAttachmentsAsync(item.Id, SourceType.RFQD, Modules.Bid);
            }

            return ApiResponse<RFQMainDto>.Success(mainRFQDto, 200);
        }

        public async Task<ApiResponse<List<RFQInProgressDto>>> GetInProgressAsync(RFQFilterBase filter, string userName)
        {
            int userId = await _userRepository.ConvertIdentity(userName);
            var inProgressRFQS = await _repository.GetInProgressesAsync(filter, userId);
            var dto = _mapper.Map<List<RFQInProgressDto>>(inProgressRFQS);
            return ApiResponse<List<RFQInProgressDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<bool>> DeleteAsync(List<int> rfqMainIds, string userIdentity)
        {
            var deleteTasks = rfqMainIds.Select(async rfqMainId =>
            {
                var response = await _repository.DeleteMainsync(rfqMainId, Convert.ToInt32(userIdentity));
                return response != null;
            }).ToList();

            await Task.WhenAll(deleteTasks);
            bool allDeleted = deleteTasks.All(task => task.Result);

            await _unitOfWork.SaveChangesAsync();
            return allDeleted ? ApiResponse<bool>.Success(true, 200) : ApiResponse<bool>.Fail(false, 400);
        }

        public async Task<ApiResponse<List<Application.Dtos.RFQ.UOMDto>>> GetPUOMAsync(int businessUnitId,
            string itemCodes)
        {
            var puomList = await _repository.GetPUOMAsync(businessUnitId, itemCodes);
            var dto = _mapper.Map<List<Application.Dtos.RFQ.UOMDto>>(puomList);

            return ApiResponse<List<Application.Dtos.RFQ.UOMDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<int>> RFQVendorIUDAsync(RFQVendorIUDDto dto, string userIdentity)
        {
            var mainRFQ = await _repository.GetRFQMainAsync(dto.Id);

            if (mainRFQ.ProcurementType == ProcurementType.Bidding && dto.VendorCodes.Count < 2)
                return ApiResponse<int>.Fail("Vendor Code must be greater than 2", 400);

            var data = _mapper.Map<RFQVendorIUD>(dto);
            var result = await _repository.RFQVendorIUDAsync(data, Convert.ToInt32(userIdentity));

            await _unitOfWork.SaveChangesAsync();

            List<RfqVendorToSend> mailData = new List<RfqVendorToSend>();

            foreach (var vendorCode in dto.VendorCodes)
            {
                var vendor = await _vendorRepository.GetRevisionVendorIdAndNameByVendorCode(vendorCode);

                var users = await _userRepository.GetVendorUsersForMail(vendor.VendorId);

                foreach (var user in users)
                {
                    mailData.Add(new RfqVendorToSend()
                    {
                        VendorId = vendor.VendorId,
                        VendorCode = vendor.VendorCode,
                        VendorName = vendor.VendorName,
                        Email = user.Email,
                        Language = user.Language,
                        RFQMainId = mainRFQ.Id,
                        RFQDeadline = mainRFQ.RFQDeadline,
                        RFQNo = mainRFQ.RFQNo,
                    });
                }
            }

            _taskQueue.QueueBackgroundWorkItem(async token =>
            {
                await _mailService.SendRFQVendorApproveMail(mailData);
            });

            return result ? ApiResponse<int>.Success(1, 200) : ApiResponse<int>.Fail(0, 400);
        }

        public async Task<ApiResponse<List<RFQVendorsDto>>> GetRfqVendors(int rfqMainId)
        {
            var rfqVendors = await _repository.GetRfqVendors(rfqMainId);
            var dto = _mapper.Map<List<RFQVendorsDto>>(rfqVendors);

            return ApiResponse<List<RFQVendorsDto>>.Success(dto, 200);
        }

        public async Task<bool> ChangeRFQVendorResponseStatus(int rfqMainId, string vendorCode)
        {
            var result = await _repository.ChangeRFQVendorResponseStatus(rfqMainId, vendorCode);

            return result;
        }

        public async Task GetRFQDeadlineFinished()
        {
            List<RFQDeadlineFinishedMailForBuyerDto> rfqs = await _repository.GetRFQDeadlineFinished();

            if (rfqs.Count > 0)
            {
                var rfqMainIds = rfqs.Select(x => x.RFQMainId).ToList();
                var idListForSql = string.Join(",", rfqMainIds);

                using (var command = _unitOfWork.CreateCommand() as DbCommand)
                {
                    command.CommandText =
                        @$"set nocount off update Procurement.RFQMain set Status = 2 where RFQMainId in ({idListForSql})";
                    await command.ExecuteNonQueryAsync();
                    Console.WriteLine("RFQMain update edildi");
                }

                await _unitOfWork.SaveChangesAsync();

                foreach (var rfq in rfqs.ToList())
                {
                    string buyerEmail =
                        await _buyerService.FindBuyerEmailByBuyerName(rfq.BuyerName, rfq.BusinessUnitId);
                    rfq.BuyerEmail = buyerEmail;
                }

                List<RFQVendorEmailDto> vendorEmails = await _repository.GetRfqVendors(rfqMainIds);
                Console.WriteLine("vendorEmails tapıldı");
                _taskQueue.QueueBackgroundWorkItem(async token =>
                {
                    await _mailService.SendRFQDeadlineFinishedMailForBuyer(rfqs);
                    await _mailService.RFQCloseSendVendorEmail(vendorEmails);
                });
            }
        }

        public async Task<ApiResponse<bool>> ExtendRfqDeadlineAsync(RfqExtendDeadlineRequest request, int userId)
        {
            var result = await _repository.ExtendRfqDeadlineAsync(request, userId);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(result);
        }

        public async Task SendRFQDeadLineMail()
        {
            RFQMethods methods = new RFQMethods(_unitOfWork);

            List<RFQUserData> rfqs = await _repository.GetRFQVendorUsersMailIsSentDeadLineFalse();

            if (rfqs.Count > 0)
            {
                _taskQueue.QueueBackgroundWorkItem(async token => { await _mailService.SendRFQDeadLineMail(rfqs); });

                await methods.UpdateMailIsSentDeadLine(rfqs.Select(x => x.RFQVendorResponseId).Distinct().ToList());
            }
        }

        public async Task SendRFQLastDayMail()
        {
            RFQMethods methods = new RFQMethods(_unitOfWork);

            List<RFQUserData> rfqs = await _repository.GetRFQVendorUsersMailIsSentLastDayFalse();

            if (rfqs.Count > 0)
            {
                _taskQueue.QueueBackgroundWorkItem(async token => { await _mailService.SendRFQLastDayMail(rfqs); });

                await methods.UpdateMailIsSentLastDay(rfqs.Select(x => x.RFQVendorResponseId).Distinct().ToList());
            }
        }

        public async Task<List<RFQVendorEmailDto>> GetRfqVendors(List<int> rfqMainIds)
        {
            return await _repository.GetRfqVendors(rfqMainIds);
        }
    }
}