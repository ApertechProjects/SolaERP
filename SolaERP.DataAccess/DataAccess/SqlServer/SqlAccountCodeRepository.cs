using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Account;
using SolaERP.Infrastructure.Entities.Location;
using SolaERP.Infrastructure.Entities.Request;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlAccountCodeRepository : SqlBaseRepository<AccountCode>, IAccountCodeRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlAccountCodeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> AddAsync(AccountCode entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AccountCode>> GetAccountCodesByBusinessUnit(string businessUnitId)
        {
            List<AccountCode> accounts = new List<AccountCode>();
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = ReplaceQuery("[dbo].[VW_UNI_AccountCodeList]", new ReplaceParams { ParamName = "APT", Value = businessUnitId });

                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    accounts.Add(reader.GetByEntityStructure<AccountCode>());
                }
                return accounts;
            }
        }

        public async Task<List<AccountCode>> GetAllAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "Select * from dbo.VW_UNI_AccountCodeList";
                using var reader = await command.ExecuteReaderAsync();

                List<AccountCode> accountCodes = new List<AccountCode>();

                while (reader.Read())
                {
                    accountCodes.Add(reader.GetByEntityStructure<AccountCode>());
                }
                return accountCodes;
            }

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
