using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.Entities.Currency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface ICurrencyCodeRepository : ICrudOperations<Currency>
    {
        Task<List<Currency>> CurrencyCodes(string businessUnitCode);
    }
}
