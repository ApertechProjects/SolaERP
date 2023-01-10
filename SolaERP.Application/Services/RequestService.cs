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
                requestDetailDto.RequestMainId = mainId.Data;
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

        public async Task<int> SaveRequestDetailsAsync(RequestDetailDto requestDetailDto)
        {
            var entity = _mapper.Map<RequestDetail>(requestDetailDto);
            var requestDetails = await _requestDetailRepository.AddOrUpdateAsync(entity);
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
    }
}
