using SolaERP.Application.Entities.AccountCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IAccountCodeRepository : ICrudOperations<AccountCode>
    {
        Task<List<AccountCode>> GetAccountCodesByBusinessUnit(string businessUnit);
    }
}
