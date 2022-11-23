using AutoMapper;
using Microsoft.AspNetCore.Http;
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

        public Task<ApiResponse<Token>> AddAsync(BusinessUnitsAllDto model)
        {
            throw new NotImplementedException();
        }

        public Task<BusinessUnitsDto> AddAsync(BusinessUnitsDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<BusinessUnitsAllDto>>> GetAllAsync()
        {
            var businessUnits = await _businessUnitRepository.GetAllAsync();
            var dto = _mapper.Map<List<BusinessUnitsAllDto>>(businessUnits);

            return ApiResponse<List<BusinessUnitsAllDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<BusinessUnitsDto>>> GetBusinessUnitListByUserId(int userId)
        {
            var businessUnits = await _businessUnitRepository.GetBusinessUnitListByUserId(userId);
            var dto = _mapper.Map<List<BusinessUnitsDto>>(businessUnits);

            return ApiResponse<List<BusinessUnitsDto>>.Success(null, 200);
        }

        public Task<ApiResponse<bool>> RemoveAsync(BusinessUnitsAllDto model)
        {
            throw new NotImplementedException();
        }


        public Task<ApiResponse<bool>> UpdateAsync(BusinessUnitsAllDto model)
        {
            throw new NotImplementedException();
        }

        Task<BusinessUnitsAllDto> ICrudService<BusinessUnitsAllDto>.AddAsync(BusinessUnitsAllDto model)
        {
            throw new NotImplementedException();
        }

      
    }
}
