using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Account;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Persistence.Services
{
    public class AccountCodeService : IAccountCodeService
    {
        private readonly IAccountCodeRepository _accountCodeRepository;
        private IMapper _mapper;
        public AccountCodeService(IAccountCodeRepository accountCodeRepository, IMapper mapper)
        {
            _mapper = mapper;
            _accountCodeRepository = accountCodeRepository;
        }

        public Task AddAsync(AccountCodeDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<AccountCodeDto>>> GetAccountCodesByBusinessUnit(string businessUnit)
        {
            var accountCodes = await _accountCodeRepository.GetAccountCodesByBusinessUnit(businessUnit);
            var dto = _mapper.Map<List<AccountCodeDto>>(accountCodes);
            return ApiResponse<List<AccountCodeDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<AccountCodeDto>>> GetAllAsync()
        {
            var accountCodes = await _accountCodeRepository.GetAllAsync();
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
