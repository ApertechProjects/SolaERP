using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlAccountCodeRepository : IAccountCodeRepository
    {
        public Task<bool> AddAsync(AccountCode entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<AccountCode>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AccountCode> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(AccountCode entity)
        {
            throw new NotImplementedException();
        }
    }
}
