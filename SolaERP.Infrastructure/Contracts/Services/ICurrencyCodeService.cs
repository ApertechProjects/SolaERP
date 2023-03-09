using SolaERP.Infrastructure.Dtos.Currency;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface ICurrencyCodeService : ICrudService<CurrencyDto>
    {
        Task<ApiResponse<List<CurrencyDto>>> GetCurrencyCodesByBusinessUnitId(string businessUnitCode);
    }
}
