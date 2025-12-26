using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.GNRLConfig;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Persistence.Services
{
    public class GNRLConfigService : IGNRLConfigService
    {
        private readonly IGNRLConfigRepository _repository;
        private readonly IMapper _mapper;

        public GNRLConfigService(IGNRLConfigRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<GNRLConfigDto>>> GetGNRLListByBusinessUnitId(int businessUnitId)
        {
            var configs = await _repository.GetGNRLConfigsByBusinessUnitId(businessUnitId);
            var dto = _mapper.Map<List<GNRLConfigDto>>(configs);
            return ApiResponse<List<GNRLConfigDto>>.Success(dto, 200);
        }
    }
}