using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.BusinessUnit;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Application.Services
{
    public class BusinessUnitService : IBusinessUnitService
    {
        private readonly IBusinessUnitRepository _businessUnitRepository;
        private readonly IUserRepository _userRepository;
        private IMapper _mapper;

        public BusinessUnitService(IBusinessUnitRepository businessUnitRepository, IUserRepository userRepository, IMapper mapper)
        {
            _businessUnitRepository = businessUnitRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public Task AddAsync(BusinessUnitsAllDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<BusinessUnitsAllDto>>> GetAllAsync()
        {
            var businessUnits = await _businessUnitRepository.GetAllAsync();
            var dto = _mapper.Map<List<BusinessUnitsAllDto>>(businessUnits);

            return ApiResponse<List<BusinessUnitsAllDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<BusinessUnitsDto>>> GetBusinessUnitListByUserId(string finderToken)
        {
            var businessUnits = await _businessUnitRepository.GetBusinessUnitListByUserId(
                await _userRepository.GetUserIdByTokenAsync(finderToken));

            var dto = _mapper.Map<List<BusinessUnitsDto>>(businessUnits);

            return ApiResponse<List<BusinessUnitsDto>>.Success(null, 200);
        }

        public Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }


        public Task<ApiResponse<bool>> UpdateAsync(BusinessUnitsAllDto model)
        {
            throw new NotImplementedException();
        }

        Task ICrudService<BusinessUnitsAllDto>.AddAsync(BusinessUnitsAllDto model)
        {
            throw new NotImplementedException();
        }
    }
}
