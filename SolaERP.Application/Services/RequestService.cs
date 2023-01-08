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


        public Task<int> AddOrUpdate(RequestMainDto dto)
        {
            throw new NotImplementedException();
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
            var mainRequest = await _requestMainRepository.GetAllAsync();
            var mainRequestDto = _mapper.Map<List<RequestMainDto>>(mainRequest);

            return mainRequestDto;
        }

        public Task<RequestMainDto> GetByIdAsync(int Id)
        {
            throw new NotImplementedException();
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
