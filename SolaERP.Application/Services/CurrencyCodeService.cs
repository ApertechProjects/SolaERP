using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Currency;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Persistence.Services
{
    public class CurrencyCodeService : ICurrencyCodeService
    {
        private readonly ICurrencyCodeRepository _currencyCodeRepository;
        private IMapper _mapper;
        public CurrencyCodeService(ICurrencyCodeRepository currencyRepository, IMapper mapper)
        {
            _currencyCodeRepository = currencyRepository;
            _mapper = mapper;
        }
        public Task AddAsync(CurrencyDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<CurrencyDto>>> GetAllAsync()
        {
            var currCodes = await _currencyCodeRepository.GetAllAsync();
            var dto = _mapper.Map<List<CurrencyDto>>(currCodes);
            return ApiResponse<List<CurrencyDto>>.Success(dto, 200);
        }

        public Task<ApiResponse<List<CurrencyDto>>> GetAllAsync(string businessUnitCode)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<CurrencyDto>>> GetCurrencyCodesByBusinessUnitCode(string businessUnitCode)
        {
            var currCodes = await _currencyCodeRepository.CurrencyCodes(businessUnitCode);
            var dto = _mapper.Map<List<CurrencyDto>>(currCodes);
            return ApiResponse<List<CurrencyDto>>.Success(dto, 200);
        }

        public Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateAsync(CurrencyDto model)
        {
            throw new NotImplementedException();
        }
    }
}
