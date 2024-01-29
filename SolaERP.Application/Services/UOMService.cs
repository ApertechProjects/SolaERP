using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.UOM;

namespace SolaERP.Persistence.Services
{
    public class UOMService : IUOMService
    {
        private readonly IUOMRepository _uomRepository;
        private IMapper _mapper;
        public UOMService(IUOMRepository uomRepository, IMapper mapper)
        {
            _uomRepository = uomRepository;
            _mapper = mapper;
        }
        public Task AddAsync(UOMDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<UOMDto>>> GetAllAsync()
        {
            var uoms = await _uomRepository.GetAllAsync();
            var dto = _mapper.Map<List<UOMDto>>(uoms);
            return ApiResponse<List<UOMDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<UOMDto>>> GetUOMListBusinessUnitCode(int businessUnitId)
        {
            var uoms = await _uomRepository.GetUOMListBusinessUnitCode(businessUnitId);
            var dto = _mapper.Map<List<UOMDto>>(uoms);
            return ApiResponse<List<UOMDto>>.Success(dto, 200);
        }

        public Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateAsync(UOMDto model)
        {
            throw new NotImplementedException();
        }
    }
}
