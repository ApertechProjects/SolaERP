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

        public RequestService(IUnitOfWork unitOfWork, IMapper mapper, IRequestMainRepository requestMainRepository, IRequestDetailRepository requestDetailRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _requestMainRepository = requestMainRepository;
            _requestDetailRepository = requestDetailRepository;
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

        public bool DeleteAsync(RequestMain entity)
        {
            throw new NotImplementedException();
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

        public async Task<bool> SaveRequestDetailsAsync(RequestDetailDto requestDetailDto)
        {
            var entity = _mapper.Map<RequestDetail>(requestDetailDto);
            var result = await _requestDetailRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public Task<ApiResponse<List<RequestMainWithDetailsDto>>> GetAllMainRequetsWithDetails()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<RequestMainWithDetailsDto>> GetRequetsMainWithDetailsById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
