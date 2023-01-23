using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Request;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities.Request;
using SolaERP.Infrastructure.UnitOfWork;
using SolaERP.Infrastructure.ViewModels;

namespace SolaERP.Application.Services
{
    public class RequestService : IRequestService
    {
        public IUnitOfWork _unitOfWork;
        public IMapper _mapper;
        public IRequestMainRepository _requestMainRepository;
        public IRequestDetailRepository _requestDetailRepository;

        public RequestService(IUnitOfWork unitOfWork, IMapper mapper, IRequestMainRepository requestMainRepository, IRequestDetailRepository requestDetailRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _requestMainRepository = requestMainRepository;
            _requestDetailRepository = requestDetailRepository;
        }


        public async Task<ApiResponse<int>> AddOrUpdate(RequestMainDto dto)
        {
            var entity = _mapper.Map<RequestMain>(dto);
            var result = await _requestMainRepository.AddOrUpdateAsync(entity);

            return result > 0 ? ApiResponse<int>.Success(result, 200) : ApiResponse<int>.Fail("Something went wrong. Please contact with us", 400);
        }

        public Task<int> DeleteAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveRequestDetailAsync(RequestDetailDto requestDetailDto)
        {
            var entity = _mapper.Map<RequestDetail>(requestDetailDto);
            var model = await _requestDetailRepository.RemoveAsync(entity.RequestMainId);
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

        public async Task<ApiResponse<RequestSaveVM>> SaveRequest(RequestSaveVM requestSaveVM)
        {
            var mainId = await _requestMainRepository.AddOrUpdateAsync(_mapper.Map<RequestMain>(requestSaveVM.RequestMainDto));

            for (int i = 0; i < requestSaveVM.RequestDetailDtos.Count; i++)
            {
                var requestDetailDto = requestSaveVM.RequestDetailDtos[i];
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

            return ApiResponse<RequestSaveVM>.Success(requestSaveVM, 200);
        }

        public async Task<bool> SaveRequestDetailsAsync(RequestDetailDto requestDetailDto)
        {
            var entity = _mapper.Map<RequestDetail>(requestDetailDto);
            var result = await _requestDetailRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<ApiResponse<List<RequestWFADto>>> GetWaitingForApprovalsAsync(RequestWFAGetParametersDto requestWFAGetParametersDto)
        {
            var mainRequest = await _requestMainRepository.GetWaitingForApprovalsAsync(requestWFAGetParametersDto.UserId, requestWFAGetParametersDto.BusinessUnitId, requestWFAGetParametersDto.DateFrom, requestWFAGetParametersDto.DateTo, requestWFAGetParametersDto.ItemCode);
            var mainRequestDto = _mapper.Map<List<RequestWFADto>>(mainRequest);

            if (mainRequestDto != null && mainRequestDto.Count > 0)
                return ApiResponse<List<RequestWFADto>>.Success(mainRequestDto, 200);

            return ApiResponse<List<RequestWFADto>>.Fail("Bad Request", 404);
        }

        public async Task<ApiResponse<RequestMainWithDetailsDto>> GetRequestByRequestMainId(int requestMainId)
        {
            var request = await _requestMainRepository.GetRequestByRequestMainId(requestMainId);
            var requestDto = _mapper.Map<RequestMainWithDetailsDto>(requestMainId);

            return ApiResponse<RequestMainWithDetailsDto>.Success(requestDto, 200);
        }
    }
}
