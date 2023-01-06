using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Request;
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

        public Task<RequestMainDto> GetByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
