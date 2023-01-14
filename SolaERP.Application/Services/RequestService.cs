using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Request;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities.Request;
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

        public async Task<ApiResponse<RequestMainDto>> AddOrUpdateAsync(RequestMainDto requestMainDto)
        {
            var mainId = await _requestMainRepository.AddOrUpdateAsync(_mapper.Map<RequestMain>(requestMainDto));

            if (mainId != 0)
            {
                for (int i = 0; i < requestMainDto.RequestDetailDtos.Count; i++)
                {
                    var requestDetailDto = requestMainDto.RequestDetailDtos[i];
                    requestDetailDto.RequestMainId = mainId;
                    if (requestDetailDto.Type == "remove")
                    {
                        await RemoveRequestDetailAsync(requestDetailDto);
                    }
                    else
                    {
                        await SaveRequestDetailsAsync(requestDetailDto);
                    }
                }
                return ApiResponse<RequestMainDto>.Success(requestMainDto, 200);
            }
            return ApiResponse<RequestMainDto>.Fail("Not Found", 404);
        }

        public async Task<ApiResponse<bool>> ChangeRequestStatus(List<RequestChangeStatusParametersDto> changeStatusParametersDtos)
        {
            var userId = await _userRepository.GetUserIdByTokenAsync(changeStatusParametersDtos[0].FinderToken);
            for (int i = 0; i < changeStatusParametersDtos.Count; i++)
            {
                changeStatusParametersDtos[i].UserId = userId;
                await _requestMainRepository.ChangeRequestStatusAsync(changeStatusParametersDtos[i]);
            }
            return ApiResponse<bool>.Success(200);
        }

        public Task<int> DeleteAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<bool>> SendMainToApproveAsync(RequestMainSendToApproveDto sendToApproveModel)
        {
            var result = await _requestMainRepository.SendRequestToApproveAsync(sendToApproveModel.UserId, sendToApproveModel.RequestMainId);
            return result ? ApiResponse<bool>.Success(204) : ApiResponse<bool>.Fail("Requst not approved", 400);
        }

        public Task<ApiResponse<List<RequestMainWithDetailsDto>>> GetAllMainRequetsWithDetails()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<RequestMainWithDetailsDto>> GetRequetsMainWithDetailsById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<RequestMainDraftDto>>> GetAllRequestMainDraftsAsync(RequestMainDraftGetDto getMainDraftParameters)
        {
            var mainDraftEntites = await _requestMainRepository.GetAllMainRequestDraftsAsync(getMainDraftParameters.BusinessUnitId, getMainDraftParameters.ItemCode, getMainDraftParameters.DateFrom, getMainDraftParameters.DateTo);
            var mainDraftDto = _mapper.Map<List<RequestMainDraftDto>>(mainDraftEntites);

            if (mainDraftEntites.Count > 0)
                return ApiResponse<List<RequestMainDraftDto>>.Success(mainDraftDto, 200);

            return ApiResponse<List<RequestMainDraftDto>>.Fail("Error not found", 404);
        }
    }

}
