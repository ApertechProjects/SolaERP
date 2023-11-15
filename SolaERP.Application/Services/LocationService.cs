using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Location;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Persistence.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private IMapper _mapper;

        public LocationService(ILocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public Task AddAsync(LocationDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<LocationDto>>> GetAllAsync()
        {
            var locations = await _locationRepository.GetAllAsync();
            var dto = _mapper.Map<List<LocationDto>>(locations);
            return ApiResponse<List<LocationDto>>.Success(dto, 200);
        }

        public Task<ApiResponse<List<LocationDto>>> GetLocationListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<LocationDto>>> GetAllByBusinessUnitId(int businessUnitId)
        {
            var entityList = await _locationRepository.GetAllByBusinessUnitId(businessUnitId);
            var dto = _mapper.Map<List<LocationDto>>(entityList);
            return ApiResponse<List<LocationDto>>.Success(dto, 200);
        }

        public Task<ApiResponse<bool>> UpdateAsync(LocationDto model)
        {
            throw new NotImplementedException();
        }
    }
}