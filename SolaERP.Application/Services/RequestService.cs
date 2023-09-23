using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Request;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.Request;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.Persistence.Utils;
using System.Xml.Linq;
using SolaERP.Application.Dtos.Attachment;
using SolaERP.Application.Enums;

namespace SolaERP.Persistence.Services
{
    public class RequestService : IRequestService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IRequestMainRepository _requestMainRepository;
        private IRequestDetailRepository _requestDetailRepository;
        private IUserRepository _userRepository;
        private IMailService _mailService;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IFileUploadService _fileUploadService;

        public RequestService(IUnitOfWork unitOfWork, IMapper mapper, IRequestMainRepository requestMainRepository,
            IRequestDetailRepository requestDetailRepository, IUserRepository userRepository, IMailService mailService,
            IEmailNotificationService emailNotificationService, IAttachmentRepository attachmentRepository, IFileUploadService fileUploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _requestMainRepository = requestMainRepository;
            _requestDetailRepository = requestDetailRepository;
            _userRepository = userRepository;
            _mailService = mailService;
            _emailNotificationService = emailNotificationService;
            _attachmentRepository = attachmentRepository;
            _fileUploadService = fileUploadService;
        }

        public async Task<bool> RemoveDetailAsync(int requestDetailId)
        {
            var result = await _requestDetailRepository.RemoveAsync(requestDetailId);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<ApiResponse<List<RequestMainDto>>> GetAllAsync(RequestMainGetModel model)
        {
            var mainRequest = await _requestMainRepository.GetAllAsync(model);
            var mainRequestDto = _mapper.Map<List<RequestMainDto>>(mainRequest);

            if (mainRequestDto != null && mainRequestDto.Count > 0)
                return ApiResponse<List<RequestMainDto>>.Success(mainRequestDto, 200);

            return ApiResponse<List<RequestMainDto>>.Success(mainRequestDto, 200);
        }

        public async Task<bool> SaveRequestDetailsAsync(RequestDetailDto requestDetailDto)
        {
            var entity = _mapper.Map<RequestDetail>(requestDetailDto);
            entity.RequestDate = entity.RequestDate.ConvertDateToValidDate();
            var requestDetails = await _requestDetailRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return requestDetails;
        }

        public async Task<ApiResponse<List<RequestTypesDto>>> GetTypesAsync(int businessUnitId)
        {
            var entity = await _requestMainRepository.GetRequestTypesByBusinessUnitIdAsync(businessUnitId);
            var dto = _mapper.Map<List<RequestTypesDto>>(entity);

            return entity.Count > 0
                ? ApiResponse<List<RequestTypesDto>>.Success(dto, 200)
                : ApiResponse<List<RequestTypesDto>>.Fail("Request types not found", 404);
        }

        public async Task<bool> ChangeMainStatusAsync(string name, int requestMainId, int approveStatus, string comment,
            int rejectReasonId)
        {
            var userId = await _userRepository.ConvertIdentity(name);

            List<string> failedMailList = new List<string>();
            var user = await _userRepository.GetByIdAsync(userId);

            var result = await _requestMainRepository.RequestMainChangeStatusAsync(userId, requestMainId, approveStatus,
                comment, rejectReasonId);


            await _unitOfWork.SaveChangesAsync();

            return result;
        }


        public async Task<ApiResponse<bool>> SendToApproveAsync(string name, List<int> requestMainIds)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            User user = await _userRepository.GetByIdAsync(userId);
            string messageBody = "Request sent to approve by " + user.FullName;

            var sendToApproveTasks = requestMainIds.Select(async requestId =>
            {
                return await _requestMainRepository.SendRequestToApproveAsync(userId, requestId);
            }).ToList();

            await Task.WhenAll(sendToApproveTasks);
            await _unitOfWork.SaveChangesAsync();

            var followUserEmailTasks = requestMainIds.Select(GetFollowUserEmailsForRequestAsync);
            var followUserEmails = (await Task.WhenAll(followUserEmailTasks)).SelectMany(emails => emails).ToArray();

            await _mailService.SendSafeMailsAsync(followUserEmails, "Request Information", messageBody, false);

            bool allSuccess = sendToApproveTasks.All(task => task.Result);
            return allSuccess
                ? ApiResponse<bool>.Success(true, 200)
                : ApiResponse<bool>.Fail(false, 400);
        }

        public async Task<ApiResponse<RequestCardMainDto>> GetByMainId(string name, int requestMainId)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var requestMain = await _requestMainRepository.GetRequesMainHeaderAsync(requestMainId, userId);
            requestMain.requestCardDetails =
                await _requestDetailRepository.GetRequestDetailsByMainIdAsync(requestMainId);
            requestMain.requestCardAnalysis = await _requestDetailRepository.GetAnalysis(requestMainId);
            var attachments =
                await _attachmentRepository.GetAttachmentsAsync(requestMainId, null, SourceType.REQ.ToString());
            var attachmentDtoList = attachments.Select(x =>
            {
                var dto = _mapper.Map<AttachmentDto>(x);
                dto.FileLink = _fileUploadService.GetDownloadFileLink(dto.FileLink, Modules.EvaluationForm);
                return dto;
            }).ToList();
            var requestDto = _mapper.Map<RequestCardMainDto>(requestMain);
            requestDto.Attachments = attachmentDtoList;
            return ApiResponse<RequestCardMainDto>.Success(requestDto, 200);
        }

        public async Task<ApiResponse<List<RequestMainDraftDto>>> GetDraftsAsync(
            RequestMainDraftModel getMainDraftParameters)
        {
            var mainDraftEntites = await _requestMainRepository.GetMainRequestDraftsAsync(getMainDraftParameters);
            var mainDraftDto = _mapper.Map<List<RequestMainDraftDto>>(mainDraftEntites);

            if (mainDraftEntites.Count > 0)
                return ApiResponse<List<RequestMainDraftDto>>.Success(mainDraftDto, 200);

            return ApiResponse<List<RequestMainDraftDto>>.Success(mainDraftDto, 200);
            //return ApiResponse<List<RequestMainDraftDto>>.Fail("Main drafts is empty", 404);
        }

        public async Task<ApiResponse<List<RequestAmendmentDto>>> GetChangeApprovalAsync(string name,
            RequestApproveAmendmentModel requestParametersDto)
        {
            var userId = await _userRepository.ConvertIdentity(name);
            var mainRequest =
                await _requestMainRepository.GetApproveAmendmentRequestsAsync(userId, requestParametersDto);
            var mainRequestDto = _mapper.Map<List<RequestAmendmentDto>>(mainRequest);

            if (mainRequestDto != null && mainRequestDto.Count > 0)
                return ApiResponse<List<RequestAmendmentDto>>.Success(mainRequestDto, 200);

            return ApiResponse<List<RequestAmendmentDto>>.Success(mainRequestDto, 200);
            //return ApiResponse<List<RequestAmendmentDto>>.Fail("Amendment is empty", 404);
        }

        public async Task<ApiResponse<List<RequestApprovalInfoDto>>> GetApprovalInfoAsync(string name,
            int requestMainId)
        {
            var userId = await _userRepository.ConvertIdentity(name);
            var approvalInfo = await _requestMainRepository.GetRequestApprovalInfoAsync(requestMainId, userId);
            var approvalInfoResult = _mapper.Map<List<RequestApprovalInfoDto>>(approvalInfo);

            return approvalInfoResult.Count > 0
                ? ApiResponse<List<RequestApprovalInfoDto>>.Success(approvalInfoResult, 200)
                : ApiResponse<List<RequestApprovalInfoDto>>.Fail("Bad Request Approval info is empty", 404);
        }

        public async Task<ApiResponse<RequestMainDto>> GetHeaderAsync(string name, int requestMainId)
        {
            var userId = await _userRepository.ConvertIdentity(name);
            var requestHeader = await _requestMainRepository.GetRequesMainHeaderAsync(userId, requestMainId);
            var requestHeaderResult = _mapper.Map<RequestMainDto>(requestHeader);

            return requestHeaderResult != null
                ? ApiResponse<RequestMainDto>.Success(requestHeaderResult, 200)
                : ApiResponse<RequestMainDto>.Fail("Bad request header is null", 404);
        }

        public async Task<ApiResponse<List<RequestDetailsWithAnalysisCodeDto>>> GetDetails(int requestmainId)
        {
            var requestDetails = await _requestDetailRepository.GetRequestDetailsByMainIdAsync(requestmainId);
            var requestDetailsResult = _mapper.Map<List<RequestDetailsWithAnalysisCodeDto>>(requestDetails);

            return requestDetailsResult.Count > 0
                ? ApiResponse<List<RequestDetailsWithAnalysisCodeDto>>.Success(requestDetailsResult, 200)
                : ApiResponse<List<RequestDetailsWithAnalysisCodeDto>>.Fail("Request details is empty", 404);
        }

        public async Task<ApiResponse<RequestSaveResultModel>> AddOrUpdateAsync(string name, RequestSaveModel model)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            RequestSaveResultModel resultModel =
                await _requestMainRepository.AddOrUpdateRequestAsync(userId, _mapper.Map<RequestMainSaveModel>(model));
            var requestDetails = _requestDetailRepository.GetRequestDetailsByMainIdAsync(model.RequestMainId).Result
                .Select(x => x.RequestDetailId).ToList()
                .Except(model.Details.Select(x => x.RequestDetailId).ToList()
                ).ToList();
            
            model.Attachments.ForEach(attachment =>
            {
                attachment.SourceId = resultModel.RequestMainId;
                attachment.SourceType = SourceType.REQ.ToString();
                _attachmentRepository.SaveAttachmentAsync(attachment);
            });


            for (int i = 0; i < requestDetails.Count; i++) //deleting
            {
                var requestDetailId = requestDetails[i];
                await RemoveDetailAsync(requestDetailId);
            }

            if (resultModel != null)
            {
                for (int i = 0; i < model.Details.Count; i++)
                {
                    var requestDetailDto = model.Details[i];
                    requestDetailDto.RequestMainId = resultModel.RequestMainId;
                    await SaveRequestDetailsAsync(requestDetailDto);
                }

                return ApiResponse<RequestSaveResultModel>.Success(resultModel, 200);
            }

            return ApiResponse<RequestSaveResultModel>.Fail("Not Found", 404);
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string name, int requestMainId)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            int requestId = await _requestMainRepository.DeleteAsync(userId, requestMainId);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(200);
        }

        public async Task<ApiResponse<List<RequestDetailApprovalInfoDto>>> GetDetailApprvalInfoAsync(
            int requestDetaildId)
        {
            var entity = await _requestDetailRepository.GetDetailApprovalInfoAsync(requestDetaildId);
            var result = _mapper.Map<List<RequestDetailApprovalInfoDto>>(entity);

            return result != null
                ? ApiResponse<List<RequestDetailApprovalInfoDto>>.Success(result, 200)
                : ApiResponse<List<RequestDetailApprovalInfoDto>>.Success(new(), 200);
        }

        public async Task<bool> ChangeDetailStatusAsync(string name, int requestDetailId, int approveStatusId,
            string comment, int sequence, int rejectReasonId)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var res = await _requestDetailRepository.RequestDetailChangeStatusAsync(requestDetailId, userId,
                approveStatusId, comment, sequence, rejectReasonId);
            if (res)
                await _unitOfWork.SaveChangesAsync();

            return true;
        }


        public Task<int> DeleteAsync(int userId, int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<RequestWFADto>>> GetWFAAsync(string name,
            RequestWFAGetModel requestWFAGetParametersDto)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var mainreq = await _requestMainRepository.GetWaitingForApprovalsAsync(userId, requestWFAGetParametersDto);

            var mainRequestDto = _mapper.Map<List<RequestWFADto>>(mainreq);

            if (mainRequestDto != null && mainRequestDto.Count > 0)
                return ApiResponse<List<RequestWFADto>>.Success(mainRequestDto, 200);

            return ApiResponse<List<RequestWFADto>>.Success(mainRequestDto, 200);
        }

        public async Task<ApiResponse<bool>> UpdateBuyerAsync(List<RequestSetBuyer> requestSetBuyer)
        {
            var data = false;
            for (int i = 0; i < requestSetBuyer.Count; i++)
            {
                data = await _requestMainRepository.UpdateBuyerAsync(requestSetBuyer[0]);
            }

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(data, 200);
        }

        public async Task<ApiResponse<List<RequestFollowDto>>> GetFollowUsersAsync(int requestMainId)
        {
            var data = await _requestMainRepository.RequestFollowUserLoadAsync(requestMainId);
            var dto = _mapper.Map<List<RequestFollowDto>>(data);
            if (dto != null && dto.Count > 0)
                return ApiResponse<List<RequestFollowDto>>.Success(dto, 200);
            return ApiResponse<List<RequestFollowDto>>.Fail("Request Follow User List is empty", 400);
        }

        public async Task<ApiResponse<bool>> SaveFollowUserAsync(RequestFollowSaveModel saveModel)
        {
            bool result = await _requestMainRepository.RequestFollowCheckUserExistAsync(saveModel);
            if (!result)
            {
                result = await _requestMainRepository.RequestFollowSaveAsync(saveModel);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.Success(result, 200);
            }

            return ApiResponse<bool>.Fail("This user already exist for this request", 200);
        }

        public async Task<ApiResponse<bool>> DeleteFollowUserAsync(int requestFollowId)
        {
            bool result = false;
            result = await _requestMainRepository.RequestFollowDeleteAsync(requestFollowId);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(result, 200);
        }

        private async Task<string[]> GetFollowUserEmailsForRequestAsync(int requestMainId)
        {
            var data = await _requestMainRepository.RequestFollowUserLoadAsync(requestMainId);
            return data?.Select(x => x.Email)?.ToArray();
        }

        public async Task<ApiResponse<int>> GetDefaultApprovalStage(string keyCode, int businessUnitId)
        {
            var data = await _requestMainRepository.GetDefaultApprovalStage(keyCode, businessUnitId);
            return ApiResponse<int>.Success(data, 200);
        }

        public async Task<ApiResponse<List<RequestCategory>>> CategoryList()
        {
            var data = await _requestMainRepository.CategoryList();
            if (data.Count > 0)
                return ApiResponse<List<RequestCategory>>.Success(data, 200);
            return ApiResponse<List<RequestCategory>>.Fail("Data not found", 404);
        }

        public async Task<ApiResponse<List<RequestHeldDto>>> GetHeldAsync(RequestWFAGetModel requestMainGet)
        {
            var mainreq = await _requestMainRepository.GetHeldAsync(requestMainGet);

            var mainRequestDto = _mapper.Map<List<RequestHeldDto>>(mainreq);

            if (mainRequestDto != null && mainRequestDto.Count > 0)
                return ApiResponse<List<RequestHeldDto>>.Success(mainRequestDto, 200);

            return ApiResponse<List<RequestHeldDto>>.Success(mainRequestDto, 200);
        }
    }
}