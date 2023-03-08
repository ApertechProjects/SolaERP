using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.BusinessUnit;
using SolaERP.Infrastructure.Dtos.Currency;
using SolaERP.Infrastructure.Dtos.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Services
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
