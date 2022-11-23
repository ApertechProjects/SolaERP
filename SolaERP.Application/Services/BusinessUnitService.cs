using AutoMapper;
using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Dtos.BusinessUnit;
using SolaERP.Infrastructure.Repositories;
using SolaERP.Infrastructure.Services;

namespace SolaERP.Application.Services
{
    public class BusinessUnitService : IBusinessUnitService
    {
        private readonly IBusinessUnitRepository _businessUnitRepository;
        private IMapper _mapper;

        public BusinessUnitService(IBusinessUnitRepository businessUnitRepository, IMapper mapper)
        {
            _businessUnitRepository = businessUnitRepository;
            _mapper = mapper;
        }

        public Task<ApiResponse<Token>> AddAsync(BusinessUnitsDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<BusinessUnitsDto>>> GetAllAsync()
        {
            var businessUnits = await _businessUnitRepository.GetAllAsync();
            var dto = _mapper.Map<List<BusinessUnitsDto>>(businessUnits);

            return ApiResponse<List<BusinessUnitsDto>>.Success(dto, 200);
        }

        public Task<ApiResponse<bool>> RemoveAsync(BusinessUnitsDto model)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateAsync(BusinessUnitsDto model)
        {
            throw new NotImplementedException();
        }
    }
}
