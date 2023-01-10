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

        public bool RemoveRequestDetailAsync(RequestDetailDto requestDetailDto)
        {
            var entity = _mapper.Map<RequestDetail>(requestDetailDto);
            var model = _requestDetailRepository.RemoveRequestDetailAsync(entity.RequestMainId);
            return model;
        }

        public async Task<List<RequestMainDto>> GetAllAsync()
        {
            var mainRequest = await _requestMainRepository.GetAllAsync(getParametersDto.BusinessUnitId, getParametersDto.ItemCode, getParametersDto.DateFrom, getParametersDto.DateTo, getParametersDto.ApproveStatus, getParametersDto.Status);
            var mainRequestDto = _mapper.Map<List<RequestMainDto>>(mainRequest);

            if (mainRequestDto != null && mainRequestDto.Count > 0)
                return ApiResponse<List<RequestMainDto>>.Success(mainRequestDto, 200);

            else
            {
                return ApiResponse<List<RequestMainDto>>.Fail("Bad Request", 404);
            }
        }

        public async Task<ApiResponse<RequestSaveVM>> SaveRequest(RequestSaveVM requestSaveVM)
        {
            var mainId = await AddOrUpdate(requestSaveVM.RequestMainDto);

            for (int i = 0; i < requestSaveVM.RequestDetailDtos.Count; i++)
            {
                var requestDetailDto = requestSaveVM.RequestDetailDtos[i];
                requestDetailDto.RequestMainId = mainId;
                if (requestDetailDto.Type == "remove")
                {
                    RemoveRequestDetailAsync(requestDetailDto);
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
    }
}
