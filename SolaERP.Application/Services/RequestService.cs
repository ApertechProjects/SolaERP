using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Request;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Application.Services
{
    public class RequestService : IRequestService
    {
        public IUnitOfWork _unitOfWork;
        public IMapper _mapper;
        public IRequestMainRepository _requestMainRepository;

        public RequestService(IUnitOfWork unitOfWork, IMapper mapper, IRequestMainRepository requestMainRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _requestMainRepository = requestMainRepository;
        }


        public Task<int> AddOrUpdate(RequestMainDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<RequestMainDto>> GetAllAsync()
        {
            var mainRequest = await _requestMainRepository.GetAllAsync();
            var mainRequestDto = _mapper.Map<List<RequestMainDto>>(mainRequest);

            return mainRequestDto;
        }

        public async Task<ApiResponse<List<RequestMainDto>>> GetAllAsync(RequestMainGetParametersDto getParametersDto)
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

        public Task<RequestMainDto> GetByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
