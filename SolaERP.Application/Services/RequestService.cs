using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Request;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities.Request;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Application.Services
{
    public class RequestService : IRequestService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IRequestMainRepository _requestMainRepository;
        private IRequestDetailRepository _requestDetailRepository;
        private IUserRepository _userRepository;
        private IMailService _mailService;
        public RequestService(IUnitOfWork unitOfWork, IMapper mapper, IRequestMainRepository requestMainRepository, IRequestDetailRepository requestDetailRepository, IUserRepository userRepository, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _requestMainRepository = requestMainRepository;
            _requestDetailRepository = requestDetailRepository;
            _userRepository = userRepository;
            _mailService = mailService;
        }

        public async Task<bool> RemoveRequestDetailAsync(int requestDetailId)
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
            //return ApiResponse<List<RequestMainDto>>.Fail("Bad Request", 404);
        }

        public async Task<bool> SaveRequestDetailsAsync(RequestDetailDto requestDetailDto)
        {
            var entity = _mapper.Map<RequestDetail>(requestDetailDto);
            var requestDetails = await _requestDetailRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return requestDetails;
        }

        public async Task<ApiResponse<List<RequestTypesDto>>> GetRequestTypesByBusinessUnitIdAsync(int businessUnitId)
        {
            var entity = await _requestMainRepository.GetRequestTypesByBusinessUnitIdAsync(businessUnitId);
            var dto = _mapper.Map<List<RequestTypesDto>>(entity);

            return entity.Count > 0 ? ApiResponse<List<RequestTypesDto>>.Success(dto, 200) :
                ApiResponse<List<RequestTypesDto>>.Fail("Request types not found", 404);
        }

        public async Task<ApiResponse<bool>> RequestMainChangeStatusAsync(string name, RequestChangeStatusModel changeStatusParametersDtos)
        {
            var userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            if (changeStatusParametersDtos.RequestMainIds == null && changeStatusParametersDtos.RequestMainIds.Count == 0)
                return ApiResponse<bool>.Fail("Request must be selected", 200);

            List<string> failedMailList = new List<string>();
            string userName = await _userRepository.GetUserNameByTokenAsync(name);
            for (int i = 0; i < changeStatusParametersDtos.RequestMainIds.Count; i++)
            {
                await _requestMainRepository.RequestMainChangeStatusAsync(userId, changeStatusParametersDtos.RequestMainIds[i], changeStatusParametersDtos.ApproveStatus, changeStatusParametersDtos.Comment);

                string messageBody = $"Request {GetMailText(changeStatusParametersDtos.ApproveStatus)} by " + userName;
                await _mailService.SendSafeMailsAsync(await GetFollowUserEmailsForRequestAsync(changeStatusParametersDtos.RequestMainIds[i]), "Request Information", messageBody, false);

            }
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.Success(true, 200);
        }

        string GetMailText(int approveStatus)
        {
            string text = "";
            switch (approveStatus)
            {
                case 2:
                    text = "approved";
                    break;
                case 3:
                    text = "rejected";
                    break;
                default:
                    text = "";
                    break;
            }
            return text;
        }

        public async Task<ApiResponse<bool>> RequestSendToApproveAsync(string name, int requestMainId)
        {
            int userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            string userName = await _userRepository.GetUserNameByTokenAsync(name);
            var result = await _requestMainRepository.SendRequestToApproveAsync(userId, requestMainId);
            await _unitOfWork.SaveChangesAsync();

            string messageBody = "Request sended to approve by " + userName;

            await _mailService.SendSafeMailsAsync(await GetFollowUserEmailsForRequestAsync(requestMainId), "Request Information", messageBody, false);

            return ApiResponse<bool>.Success(true, 200);
        }

        public async Task<ApiResponse<RequestCardMainDto>> GetRequestByRequestMainId(string name, int requestMainId)
        {
            int userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            var requestMain = await _requestMainRepository.GetRequesMainHeaderAsync(requestMainId, userId);
            requestMain.requestCardDetails = await _requestDetailRepository.GetRequestDetailsByMainIdAsync(requestMainId);
            var requestDto = _mapper.Map<RequestCardMainDto>(requestMain);
            return ApiResponse<RequestCardMainDto>.Success(requestDto, 200);
        }

        public async Task<ApiResponse<List<RequestMainDraftDto>>> GetRequestMainDraftsAsync(RequestMainDraftModel getMainDraftParameters)
        {
            var mainDraftEntites = await _requestMainRepository.GetMainRequestDraftsAsync(getMainDraftParameters);
            var mainDraftDto = _mapper.Map<List<RequestMainDraftDto>>(mainDraftEntites);

            if (mainDraftEntites.Count > 0)
                return ApiResponse<List<RequestMainDraftDto>>.Success(mainDraftDto, 200);

            return ApiResponse<List<RequestMainDraftDto>>.Success(mainDraftDto, 200);
            //return ApiResponse<List<RequestMainDraftDto>>.Fail("Main drafts is empty", 404);
        }

        public async Task<ApiResponse<List<RequestAmendmentDto>>> GetApproveAmendmentRequests(string name, RequestApproveAmendmentModel requestParametersDto)
        {
            var userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            var mainRequest = await _requestMainRepository.GetApproveAmendmentRequestsAsync(userId, requestParametersDto);
            var mainRequestDto = _mapper.Map<List<RequestAmendmentDto>>(mainRequest);

            if (mainRequestDto != null && mainRequestDto.Count > 0)
                return ApiResponse<List<RequestAmendmentDto>>.Success(mainRequestDto, 200);

            return ApiResponse<List<RequestAmendmentDto>>.Success(mainRequestDto, 200);
            //return ApiResponse<List<RequestAmendmentDto>>.Fail("Amendment is empty", 404);
        }

        public async Task<ApiResponse<List<RequestApprovalInfoDto>>> GetRequestApprovalInfoAsync(string name, int requestMainId)
        {
            var userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            var approvalInfo = await _requestMainRepository.GetRequestApprovalInfoAsync(requestMainId, userId);
            var approvalInfoResult = _mapper.Map<List<RequestApprovalInfoDto>>(approvalInfo);

            return approvalInfoResult.Count > 0 ? ApiResponse<List<RequestApprovalInfoDto>>.Success(approvalInfoResult, 200) :
                ApiResponse<List<RequestApprovalInfoDto>>.Fail("Bad Request Approval info is empty", 404);
        }

        public async Task<ApiResponse<RequestMainDto>> GetRequestHeaderAsync(string name, int requestMainId)
        {
            var userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            var requestHeader = await _requestMainRepository.GetRequesMainHeaderAsync(userId, requestMainId);
            var requestHeaderResult = _mapper.Map<RequestMainDto>(requestHeader);

            return requestHeaderResult != null ? ApiResponse<RequestMainDto>.Success(requestHeaderResult, 200) :
                ApiResponse<RequestMainDto>.Fail("Bad request header is null", 404);
        }

        public async Task<ApiResponse<List<RequestDetailsWithAnalysisCodeDto>>> GetRequestDetails(int requestmainId)
        {
            var requestDetails = await _requestDetailRepository.GetRequestDetailsByMainIdAsync(requestmainId);
            var requestDetailsResult = _mapper.Map<List<RequestDetailsWithAnalysisCodeDto>>(requestDetails);

            return requestDetailsResult.Count > 0 ? ApiResponse<List<RequestDetailsWithAnalysisCodeDto>>.Success(requestDetailsResult, 200) :
                ApiResponse<List<RequestDetailsWithAnalysisCodeDto>>.Fail("Request details is empty", 404);
        }

        public async Task<ApiResponse<RequestSaveResultModel>> AddOrUpdateRequestAsync(string name, RequestSaveModel model)
        {
            int userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            RequestSaveResultModel resultModel = await _requestMainRepository.AddOrUpdateRequestAsync(userId, _mapper.Map<RequestMainSaveModel>(model));

            if (resultModel != null)
            {
                for (int i = 0; i < model.Details.Count; i++)
                {
                    var requestDetailDto = model.Details[i];
                    requestDetailDto.RequestMainId = resultModel.RequestMainId;
                    if (requestDetailDto.Type == "remove" && requestDetailDto.RequestDetailId > 0)
                    {
                        await RemoveRequestDetailAsync(requestDetailDto.RequestDetailId);
                    }
                    else
                    {
                        await SaveRequestDetailsAsync(requestDetailDto);
                    }
                }
                return ApiResponse<RequestSaveResultModel>.Success(resultModel, 200);
            }
            return ApiResponse<RequestSaveResultModel>.Fail("Not Found", 404);
        }

        public async Task<ApiResponse<bool>> DeleteRequestAsync(string name, int requestMainId)
        {
            int userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            int requestId = await _requestMainRepository.DeleteAsync(userId, requestMainId);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(requestId);
        }

        public async Task<ApiResponse<List<RequestDetailApprovalInfoDto>>> GetRequestDetailApprvalInfoAsync(int requestDetaildId)
        {
            var entity = await _requestDetailRepository.GetDetailApprovalInfoAsync(requestDetaildId);
            var result = _mapper.Map<List<RequestDetailApprovalInfoDto>>(entity);

            return result != null ? ApiResponse<List<RequestDetailApprovalInfoDto>>.Success(result, 200) : ApiResponse<List<RequestDetailApprovalInfoDto>>.Success(new(), 200);
        }

        public async Task<ApiResponse<NoContentDto>> RequestDetailChangeStatusAsync(string name, RequestDetailApproveModel model)
        {
            int userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            if (model.RequestDetailIds == null && model.RequestDetailIds.Count == 0)
                return ApiResponse<NoContentDto>.Fail("Request must be selected", 200);

            for (int i = 0; i < model.RequestDetailIds.Count; i++)
            {
                await _requestDetailRepository.RequestDetailChangeStatusAsync(model.RequestDetailIds[i], userId, model.ApproveStatusId, model.Comment, model.Sequence);
            }
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<NoContentDto>.Success(200);
        }


        public Task<int> DeleteAsync(int userId, int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<RequestWFADto>>> GetWaitingForApprovalsAsync(string name, RequestWFAGetModel requestWFAGetParametersDto)
        {
            int userId = await _userRepository.GetIdentityNameAsIntAsync(name);
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

        public async Task<ApiResponse<List<RequestFollowDto>>> RequestFollowUserLoadAsync(int requestMainId)
        {
            var data = await _requestMainRepository.RequestFollowUserLoadAsync(requestMainId);
            var dto = _mapper.Map<List<RequestFollowDto>>(data);
            if (dto != null && dto.Count > 0)
                return ApiResponse<List<RequestFollowDto>>.Success(dto, 200);
            return ApiResponse<List<RequestFollowDto>>.Fail("Request Follow User List is empty", 400);
        }

        public async Task<ApiResponse<bool>> RequestFollowSaveAsync(RequestFollowSaveModel saveModel)
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

        public async Task<ApiResponse<bool>> RequestFollowDeleteAsync(int requestFollowId)
        {
            bool result = false;
            result = await _requestMainRepository.RequestFollowDeleteAsync(requestFollowId);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(result, 200);
        }

        public Task SendFollowMailForRequest(string[] tos, string messageBody, string subject)
        {
            throw new NotImplementedException();
        }


        private async Task<string[]> GetFollowUserEmailsForRequestAsync(int requestMainId)
        {
            var data = await _requestMainRepository.RequestFollowUserLoadAsync(requestMainId);
            return data.Select(x => x.Email).ToArray();
        }

    }
}
