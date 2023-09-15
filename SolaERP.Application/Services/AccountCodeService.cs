using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Account;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Persistence.Services
{
    public class AccountCodeService : IAccountCodeService
    {
        private readonly IAccountCodeRepository _accountCodeRepository;
        private readonly IBusinessUnitRepository _businessUnitRepository;
        private IMapper _mapper;
        public AccountCodeService(IAccountCodeRepository accountCodeRepository, IMapper mapper, IBusinessUnitRepository businessUnitRepository)
        {
            _mapper = mapper;
            _businessUnitRepository = businessUnitRepository;
            _accountCodeRepository = accountCodeRepository;
        }

        public async Task<ApiResponse<List<AccountCodeDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(AccountCodeDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<AccountCodeDto>>> GetAccountCodesByBusinessUnit(string businessUnitCode)
        {
            var businessUnitList =await  _businessUnitRepository.GetAllAsync();
            var businessUnitId = businessUnitList
                .SingleOrDefault(x => x.BusinessUnitCode == businessUnitCode).BusinessUnitId;
            var accountCodes = await _accountCodeRepository.GetAccountCodesByBusinessUnit(businessUnitId);
            var dto = _mapper.Map<List<AccountCodeDto>>(accountCodes);
            return ApiResponse<List<AccountCodeDto>>.Success(dto, 200);
        }

        public Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateAsync(AccountCodeDto model)
        {
            throw new NotImplementedException();
        }
    }
}
