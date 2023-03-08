using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Location;
using SolaERP.Infrastructure.Dtos.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Services
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

        public Task<ApiResponse<bool>> UpdateAsync(LocationDto model)
        {
            throw new NotImplementedException();
        }
    }
}
