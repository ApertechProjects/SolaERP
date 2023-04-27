using SolaERP.Application.Dtos.Currency;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Contracts.Services
{
    public interface ICurrencyCodeService : ICrudService<CurrencyDto>
    {
        Task<ApiResponse<List<CurrencyDto>>> GetCurrencyCodesByBusinessUnitCode(string businessUnitCode);
    }
}
