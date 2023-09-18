using SolaERP.Application.Entities.AccountCode;
using SolaERP.DataAccess.Extensions;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.Location;
using SolaERP.Application.Entities.Request;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlAccountCodeRepository : SqlBaseRepository, IAccountCodeRepository
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

        public async Task<List<AccountCode>> GetAccountCodesByBusinessUnit(int businessUnitId)
        {
            var accounts = new List<AccountCode>();
            await using var command = _unitOfWork.CreateCommand() as SqlCommand;
            command.CommandText = "EXEC dbo.SP_AccountList @BusinessUnitId";

            command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);

            await using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                accounts.Add(reader.GetByEntityStructure<AccountCode>());
            }
            return accounts;
        }

        public async Task<List<AccountCode>> GetAllAsync()
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
