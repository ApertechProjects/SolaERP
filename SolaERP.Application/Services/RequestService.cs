using AutoMapper;
using SolaERP.Infrastructure.Contracts.Common;
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

        public Task<int> DeleteAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveRequestDetailAsync(RequestDetailDto requestDetailDto)
        {
            var entity = _mapper.Map<RequestDetail>(requestDetailDto);
            var model = await _requestDetailRepository.RemoveAsync(entity.RequestDetailId);
            return model;
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

        public async Task<List<RequestTypesDto>> GetRequestTypesByBusinessUnitId(int businessUnitId)
        {
            var entity = await _requestMainRepository.GetRequestTypesByBusinessUnitId(businessUnitId);
            var dto = _mapper.Map<List<RequestTypesDto>>(entity);
            return dto;
        }

        async Task<ApiResponse<RequestMainDto>> IReturnableServiceMethodAsync<RequestMainDto>.AddOrUpdateAsync(RequestMainDto requestMainDto)
        {
            var mainId = await _requestMainRepository.AddOrUpdateAsync(_mapper.Map<RequestMain>(requestMainDto));

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

        Task<ApiResponse<List<RequestMainWithDetailsDto>>> IRequestService.GetAllMainRequetsWithDetails()
        {
            throw new NotImplementedException();
        }

        Task<ApiResponse<RequestMainWithDetailsDto>> IRequestService.GetRequetsMainWithDetailsById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<bool>> ChangeRequestStatus(List<RequestChangeStatusParametersDto> changeStatusParametersDtos)
        {
            var userId = await _userRepository.GetUserIdByTokenAsync(changeStatusParametersDtos[0].FinderToken);
            for (int i = 0; i < changeStatusParametersDtos.Count; i++)
            {
                await _requestMainRepository.ChangeRequestStatus(userId, changeStatusParametersDtos[i]);
            }
            return ApiResponse<bool>.Success(200);
        }

        public async Task<ApiResponse<List<RequestApproveAmendmentDto>>> GetApproveAmendmentRequests(RequestApproveAmendmentGetParametersDto requestParametersDto)
        {
            var userId = await _userRepository.GetUserIdByTokenAsync(requestParametersDto.FinderToken);
            var mainRequest = await _requestMainRepository.GetApproveAmendmentRequests(userId, requestParametersDto);
            var mainRequestDto = _mapper.Map<List<RequestApproveAmendmentDto>>(mainRequest);

            if (mainRequestDto != null && mainRequestDto.Count > 0)
                return ApiResponse<List<RequestApproveAmendmentDto>>.Success(mainRequestDto, 200);

            return ApiResponse<List<RequestApproveAmendmentDto>>.Fail("Bad Request", 404);
        }
    }

}
