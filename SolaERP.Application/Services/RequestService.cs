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
using SolaERP.Application.Enums;

namespace SolaERP.Persistence.Services
{
    public class RequestService : IRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRequestMainRepository _requestMainRepository;
        private readonly IRequestDetailRepository _requestDetailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        private readonly IAttachmentService _attachmentService;

        public RequestService(IUnitOfWork unitOfWork, IMapper mapper, IRequestMainRepository requestMainRepository,
            IRequestDetailRepository requestDetailRepository, IUserRepository userRepository, IMailService mailService,
            IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _requestMainRepository = requestMainRepository;
            _requestDetailRepository = requestDetailRepository;
            _userRepository = userRepository;
            _mailService = mailService;
            _attachmentService = attachmentService;
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
            return ApiResponse<List<RequestMainDto>>.Success(mainRequestDto);
        }

        public async Task<bool> SaveRequestDetailsAsync(RequestDetailDto requestDetailDto)
        {
            var entity = _mapper.Map<RequestDetail>(requestDetailDto);
            entity.RequestDate = entity.RequestDate.ConvertDateToValidDate();
            var requestDetails = await _requestDetailRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return requestDetails;
        }

        public async Task<ApiResponse<List<RequestTypesDto>>> GetTypesAsync(int businessUnitId, string keyCode)
        {
            var entity = await _requestMainRepository.GetRequestTypesByBusinessUnitIdAsync(businessUnitId, keyCode);
            var dto = _mapper.Map<List<RequestTypesDto>>(entity);

            return entity.Count > 0
                ? ApiResponse<List<RequestTypesDto>>.Success(dto)
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
                ? ApiResponse<bool>.Success(true)
                : ApiResponse<bool>.Fail(false, 400);
        }

        public async Task<ApiResponse<RequestCardMainDto>> GetByMainId(string name, int requestMainId)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var requestMain = await _requestMainRepository.GetRequesMainHeaderAsync(requestMainId, userId);
            requestMain.requestCardDetails =
                await _requestDetailRepository.GetRequestDetailsByMainIdAsync(requestMainId);
            requestMain.requestCardAnalysis = await _requestDetailRepository.GetAnalysis(requestMainId);
            var requestDto = _mapper.Map<RequestCardMainDto>(requestMain);
            requestDto.Attachments = await _attachmentService.GetAttachmentsAsync(requestDto.RequestMainId,
                SourceType.REQ, Modules.Request);
            return ApiResponse<RequestCardMainDto>.Success(requestDto);
        }

        public async Task<ApiResponse<List<RequestMainDraftDto>>> GetDraftsAsync(
            RequestMainDraftModel getMainDraftParameters)
        {
            var mainDraftEntites = await _requestMainRepository.GetMainRequestDraftsAsync(getMainDraftParameters);
            var mainDraftDto = _mapper.Map<List<RequestMainDraftDto>>(mainDraftEntites);
            return ApiResponse<List<RequestMainDraftDto>>.Success(mainDraftDto);
        }

        public async Task<ApiResponse<List<RequestAmendmentDto>>> GetChangeApprovalAsync(string name,
            RequestApproveAmendmentModel requestParametersDto)
        {
            var userId = await _userRepository.ConvertIdentity(name);
            var mainRequest =
                await _requestMainRepository.GetApproveAmendmentRequestsAsync(userId, requestParametersDto);
            var mainRequestDto = _mapper.Map<List<RequestAmendmentDto>>(mainRequest);
            return ApiResponse<List<RequestAmendmentDto>>.Success(mainRequestDto);
        }

        public async Task<ApiResponse<List<RequestApprovalInfoDto>>> GetApprovalInfoAsync(string name,
            int requestMainId)
        {
            var userId = await _userRepository.ConvertIdentity(name);
            var approvalInfo = await _requestMainRepository.GetRequestApprovalInfoAsync(requestMainId, userId);
            var approvalInfoResult = _mapper.Map<List<RequestApprovalInfoDto>>(approvalInfo);

            return approvalInfoResult.Count > 0
                ? ApiResponse<List<RequestApprovalInfoDto>>.Success(approvalInfoResult)
                : ApiResponse<List<RequestApprovalInfoDto>>.Fail("Bad Request Approval info is empty", 404);
        }

        public async Task<ApiResponse<RequestMainDto>> GetHeaderAsync(string name, int requestMainId)
        {
            var userId = await _userRepository.ConvertIdentity(name);
            var requestHeader = await _requestMainRepository.GetRequesMainHeaderAsync(userId, requestMainId);
            var requestHeaderResult = _mapper.Map<RequestMainDto>(requestHeader);

            return requestHeaderResult != null
                ? ApiResponse<RequestMainDto>.Success(requestHeaderResult)
                : ApiResponse<RequestMainDto>.Fail("Bad request header is null", 404);
        }

        public async Task<ApiResponse<List<RequestDetailsWithAnalysisCodeDto>>> GetDetails(int requestmainId)
        {
            var requestDetails = await _requestDetailRepository.GetRequestDetailsByMainIdAsync(requestmainId);
            var requestDetailsResult = _mapper.Map<List<RequestDetailsWithAnalysisCodeDto>>(requestDetails);

            return requestDetailsResult.Count > 0
                ? ApiResponse<List<RequestDetailsWithAnalysisCodeDto>>.Success(requestDetailsResult)
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
                if (attachment.Type == 2)
                {
                    if (attachment.AttachmentId > 0)
                    {
                        _attachmentService.DeleteAttachmentAsync(attachment.AttachmentId).Wait();
                    }
                }
                else
                {
                    if (attachment.AttachmentId > 0) return;
                    attachment.SourceId = resultModel.RequestMainId;
                    attachment.SourceType = SourceType.REQ.ToString();
                    _attachmentService.SaveAttachmentAsync(attachment).Wait();
                }
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

                return ApiResponse<RequestSaveResultModel>.Success(resultModel);
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
                ? ApiResponse<List<RequestDetailApprovalInfoDto>>.Success(result)
                : ApiResponse<List<RequestDetailApprovalInfoDto>>.Success(new List<RequestDetailApprovalInfoDto>());
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
                return ApiResponse<List<RequestWFADto>>.Success(mainRequestDto);

            return ApiResponse<List<RequestWFADto>>.Success(mainRequestDto);
        }

        public async Task<ApiResponse<bool>> UpdateBuyerAsync(List<RequestSetBuyer> requestSetBuyer)
        {
            var data = false;
            for (int i = 0; i < requestSetBuyer.Count; i++)
            {
                data = await _requestMainRepository.UpdateBuyerAsync(requestSetBuyer[0]);
            }

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(data);
        }

        public async Task<ApiResponse<List<RequestFollowDto>>> GetFollowUsersAsync(int requestMainId)
        {
            var data = await _requestMainRepository.RequestFollowUserLoadAsync(requestMainId);
            var dto = _mapper.Map<List<RequestFollowDto>>(data);
            if (dto != null && dto.Count > 0)
                return ApiResponse<List<RequestFollowDto>>.Success(dto);
            return ApiResponse<List<RequestFollowDto>>.Fail("Request Follow User List is empty", 400);
        }

        public async Task<ApiResponse<bool>> SaveFollowUserAsync(RequestFollowSaveModel saveModel)
        {
            bool result = await _requestMainRepository.RequestFollowCheckUserExistAsync(saveModel);
            if (!result)
            {
                result = await _requestMainRepository.RequestFollowSaveAsync(saveModel);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.Success(result);
            }

            return ApiResponse<bool>.Fail("This user already exist for this request", 400);
        }

        public async Task<ApiResponse<bool>> DeleteFollowUserAsync(int requestFollowId)
        {
            bool result = false;
            result = await _requestMainRepository.RequestFollowDeleteAsync(requestFollowId);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(result);
        }

        private async Task<string[]> GetFollowUserEmailsForRequestAsync(int requestMainId)
        {
            var data = await _requestMainRepository.RequestFollowUserLoadAsync(requestMainId);
            return data?.Select(x => x.Email)?.ToArray();
        }

        public async Task<ApiResponse<int>> GetDefaultApprovalStage(string keyCode, int businessUnitId)
        {
            var data = await _requestMainRepository.GetDefaultApprovalStage(keyCode, businessUnitId);
            return ApiResponse<int>.Success(data);
        }

        public async Task<ApiResponse<List<RequestCategory>>> CategoryList()
        {
            var data = await _requestMainRepository.CategoryList();
            if (data.Count > 0)
                return ApiResponse<List<RequestCategory>>.Success(data);
            return ApiResponse<List<RequestCategory>>.Fail("Data not found", 404);
        }

        public async Task<ApiResponse<List<RequestHeldDto>>> GetHeldAsync(RequestWFAGetModel requestMainGet)
        {
            var mainreq = await _requestMainRepository.GetHeldAsync(requestMainGet);

            var mainRequestDto = _mapper.Map<List<RequestHeldDto>>(mainreq);
            if (mainRequestDto != null && mainRequestDto.Count > 0)
                return ApiResponse<List<RequestHeldDto>>.Success(mainRequestDto);

            return ApiResponse<List<RequestHeldDto>>.Success(mainRequestDto);
        }
    }
}