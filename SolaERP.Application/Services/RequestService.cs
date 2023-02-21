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
        public IUnitOfWork _unitOfWork;
        public IMapper _mapper;
        public IRequestMainRepository _requestMainRepository;
        private IRequestDetailRepository _requestDetailRepository;
        private IUserRepository _userRepository;
        public RequestService(IUnitOfWork unitOfWork, IMapper mapper, IRequestMainRepository requestMainRepository, IRequestDetailRepository requestDetailRepository, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _requestMainRepository = requestMainRepository;
            _requestDetailRepository = requestDetailRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> RemoveRequestDetailAsync(RequestDetailDto requestDetailDto)
        {
            var entity = _mapper.Map<RequestDetail>(requestDetailDto);
            var result = await _requestDetailRepository.RemoveAsync(entity.RequestDetailId);
            return result;
        }

        public async Task<ApiResponse<List<RequestMainDto>>> GetAllAsync(RequestMainGetParametersDto getParametersDto)
        {
            var mainRequest = await _requestMainRepository.GetAllAsync(getParametersDto.BusinessUnitId, getParametersDto.ItemCode, getParametersDto.DateFrom, getParametersDto.DateTo, getParametersDto.ApproveStatus, getParametersDto.Status);
            var mainRequestDto = _mapper.Map<List<RequestMainDto>>(mainRequest);

            if (mainRequestDto != null && mainRequestDto.Count > 0)
                return ApiResponse<List<RequestMainDto>>.Success(mainRequestDto, 200);

            return ApiResponse<List<RequestMainDto>>.Fail("Bad Request", 404);
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

        public async Task<ApiResponse<bool>> ChangeRequestStatus(string finderToken, List<RequestChangeStatusModel> changeStatusParametersDtos)
        {
            var userId = await _userRepository.GetUserIdByTokenAsync(finderToken);
            for (int i = 0; i < changeStatusParametersDtos.Count; i++)
            {
                changeStatusParametersDtos[i].UserId = userId;
                await _requestMainRepository.ChangeRequestStatusAsync(changeStatusParametersDtos[i]);
            }
            return ApiResponse<bool>.Success(200);
        }

        public async Task<ApiResponse<bool>> SendMainToApproveAsync(RequestMainSendToApproveDto sendToApproveModel)
        {
            var result = await _requestMainRepository.SendRequestToApproveAsync(sendToApproveModel.UserId, sendToApproveModel.RequestMainId);
            return result ? ApiResponse<bool>.Success(204) : ApiResponse<bool>.Fail("Requst not approved", 400);
        }

        public async Task<ApiResponse<List<RequestWFADto>>> GetWaitingForApprovalsAsync2(string finderToken, RequestWFAGetParametersDto requestWFAGetParametersDto)
        {
            int userId = await _userRepository.GetUserIdByTokenAsync(finderToken);
            var mainRequest = await _requestMainRepository.GetWaitingForApprovalsAsync2(userId, requestWFAGetParametersDto.BusinessUnitId, requestWFAGetParametersDto.DateFrom, requestWFAGetParametersDto.DateTo, requestWFAGetParametersDto.ItemCode);
            var mainRequestDto = _mapper.Map<List<RequestWFADto>>(mainRequest);
            //int userId = await _userRepository.GetUserIdByTokenAsync(finderToken);
            //var mainreq = await _requestMainRepository.GetWaitingForApprovalsAsync(userId, requestWFAGetParametersDto.BusinessUnitId, requestWFAGetParametersDto.DateFrom, requestWFAGetParametersDto.DateTo, requestWFAGetParametersDto.ItemCode);

            ////var mainRequest = await _requestMainRepository.GetWaitingForApprovalsAsync(userId, requestWFAGetParametersDto.BusinessUnitId, requestWFAGetParametersDto.DateFrom, requestWFAGetParametersDto.DateTo, requestWFAGetParametersDto.ItemCode);
            //var mainRequestDto = _mapper.Map<List<RequestWFADto>>(mainreq);

            if (mainRequestDto != null && mainRequestDto.Count > 0)
                return ApiResponse<List<RequestWFADto>>.Success(mainRequestDto, 200);

            return ApiResponse<List<RequestWFADto>>.Fail("Waiting for approval list is empty", 404);
        }

        public async Task<ApiResponse<RequestCardMainDto>> GetRequestByRequestMainId(string authToken, int requestMainId)
        {
            int userId = await _userRepository.GetUserIdByTokenAsync(authToken);
            var requestMain = await _requestMainRepository.GetRequesMainHeaderAsync(requestMainId, userId);
            requestMain.requestCardDetails = await _requestDetailRepository.GetRequestDetailsByMainIdAsync(requestMainId);

            var requestDto = _mapper.Map<RequestCardMainDto>(requestMain);

            return ApiResponse<RequestCardMainDto>.Success(requestDto, 200);
        }

        public async Task<ApiResponse<List<RequestMainDraftDto>>> GetRequestMainDraftsAsync(RequestMainDraftModel getMainDraftParameters)
        {
            var mainDraftEntites = await _requestMainRepository.GetMainRequestDraftsAsync(getMainDraftParameters.BusinessUnitId, getMainDraftParameters.ItemCode, getMainDraftParameters.DateFrom, getMainDraftParameters.DateTo);
            var mainDraftDto = _mapper.Map<List<RequestMainDraftDto>>(mainDraftEntites);

            if (mainDraftEntites.Count > 0)
                return ApiResponse<List<RequestMainDraftDto>>.Success(mainDraftDto, 200);

            return ApiResponse<List<RequestMainDraftDto>>.Fail("Main drafts is empty", 404);
        }

        public async Task<ApiResponse<List<RequestApproveAmendmentDto>>> GetApproveAmendmentRequests(string finderToken, RequestApproveAmendmentModel requestParametersDto)
        {
            var userId = await _userRepository.GetUserIdByTokenAsync(finderToken);
            var mainRequest = await _requestMainRepository.GetApproveAmendmentRequestsAsync(userId, requestParametersDto);
            var mainRequestDto = _mapper.Map<List<RequestApproveAmendmentDto>>(mainRequest);

            if (mainRequestDto != null && mainRequestDto.Count > 0)
                return ApiResponse<List<RequestApproveAmendmentDto>>.Success(mainRequestDto, 200);

            return ApiResponse<List<RequestApproveAmendmentDto>>.Fail("Amendment is empty", 404);
        }

        public async Task<ApiResponse<List<RequestApprovalInfoDto>>> GetRequestApprovalInfoAsync(string finderToken, int requestMainId)
        {
            var userId = await _userRepository.GetUserIdByTokenAsync(finderToken);
            var approvalInfo = await _requestMainRepository.GetRequestApprovalInfoAsync(requestMainId, userId);
            var approvalInfoResult = _mapper.Map<List<RequestApprovalInfoDto>>(approvalInfo);

            return approvalInfoResult.Count > 0 ? ApiResponse<List<RequestApprovalInfoDto>>.Success(approvalInfoResult, 200) :
                ApiResponse<List<RequestApprovalInfoDto>>.Fail("Bad Request Approval info is empty", 404);
        }

        public async Task<ApiResponse<RequestMainDto>> GetRequestHeaderAsync(string finderToken, int requestMainId)
        {
            var userId = await _userRepository.GetUserIdByTokenAsync(finderToken);
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

        public async Task<ApiResponse<RequestSaveResultModel>> AddOrUpdateRequestAsync(string finderToken, RequestSaveModel model)
        {
            int userId = await _userRepository.GetUserIdByTokenAsync(finderToken);
            RequestSaveResultModel resultModel = await _requestMainRepository.AddOrUpdateRequestAsync(userId, _mapper.Map<RequestMainSaveModel>(model));

            if (resultModel != null)
            {
                for (int i = 0; i < model.Details.Count; i++)
                {
                    var requestDetailDto = model.Details[i];
                    requestDetailDto.RequestMainId = resultModel.RequestMainId;
                    if (requestDetailDto.Type == "remove")
                    {
                        await RemoveRequestDetailAsync(requestDetailDto);
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

        public async Task<ApiResponse<bool>> DeleteRequestAsync(string authToken, int requestMainId)
        {
            int userId = await _userRepository.GetUserIdByTokenAsync(authToken);
            int requestId = await _requestMainRepository.DeleteAsync(userId, requestMainId);
            return ApiResponse<bool>.Success(requestId);
        }

        public async Task<ApiResponse<RequestDetailApprovalInfoDto>> GetRequestDetailApprvalInfoAsync(int requestDetaildId)
        {
            var entity = await _requestDetailRepository.GetDetailApprovalInfoAsync(requestDetaildId);
            var result = _mapper.Map<RequestDetailApprovalInfoDto>(entity);

            return result != null ? ApiResponse<RequestDetailApprovalInfoDto>.Success(result, 200) : ApiResponse<RequestDetailApprovalInfoDto>.Success(new(), 200);
        }

        public async Task<ApiResponse<NoContentDto>> RequestDetailSendToApprove(string finderToken, RequestDetailSendToApproveModel model)
        {
            int userId = await _userRepository.GetUserIdByTokenAsync(finderToken);
            model.UserId = userId;

            await _requestDetailRepository.SendToApproveAsync(model);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<NoContentDto>.Success(200);
        }


        public Task<int> DeleteAsync(int userId, int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<RequestWFADto>>> GetWaitingForApprovalsAsync(string finderToken, RequestWFAGetParametersDto requestWFAGetParametersDto)
        {
            int userId = await _userRepository.GetUserIdByTokenAsync(finderToken);
            var mainreq = await _requestMainRepository.GetWaitingForApprovalsAsync(userId, requestWFAGetParametersDto.BusinessUnitId, requestWFAGetParametersDto.DateFrom, requestWFAGetParametersDto.DateTo, requestWFAGetParametersDto.ItemCode);

            //var mainRequest = await _requestMainRepository.GetWaitingForApprovalsAsync(userId, requestWFAGetParametersDto.BusinessUnitId, requestWFAGetParametersDto.DateFrom, requestWFAGetParametersDto.DateTo, requestWFAGetParametersDto.ItemCode);
            var mainRequestDto = _mapper.Map<List<RequestWFADto>>(mainreq);

            if (mainRequestDto != null && mainRequestDto.Count > 0)
                return ApiResponse<List<RequestWFADto>>.Success(mainRequestDto, 200);

            return ApiResponse<List<RequestWFADto>>.Fail("Waiting for approval list is empty", 404);
        }
    }
}
