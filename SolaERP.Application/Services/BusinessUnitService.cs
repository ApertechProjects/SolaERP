using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.BusinessUnit;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Persistence.Services
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

      
        public async Task<ApiResponse<List<BusinessUnitsAllDto>>> GetAllAsync()
        {
            var businessUnits = await _businessUnitRepository.GetAllAsync();
            var dto = _mapper.Map<List<BusinessUnitsAllDto>>(businessUnits);
            return ApiResponse<List<BusinessUnitsAllDto>>.Success(dto, 200);
        }


        public async Task<ApiResponse<List<BaseBusinessUnitDto>>> GetBusinessUnitListByUserToken(string finderToken)
        {
            var businessUnits = await _businessUnitRepository.GetBusinessUnitListByUserId(
                await _userRepository.GetIdentityNameAsIntAsync(finderToken));

            var dto = _mapper.Map<List<BaseBusinessUnitDto>>(businessUnits);

            return ApiResponse<List<BaseBusinessUnitDto>>.Success(dto, 200);
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
