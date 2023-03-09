using SolaERP.Infrastructure.Entities.BusinessUnits;
using SolaERP.Infrastructure.Entities.Currency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface ICurrencyCodeRepository : ICrudOperations<Currency>
    {
        Task<List<Currency>> GetCurrencyCodesByBusinessUnitCode(string businessUnitCode);
    }
}
