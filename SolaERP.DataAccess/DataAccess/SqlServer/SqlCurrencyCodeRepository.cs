using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Currency;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlCurrencyCodeRepository : SqlBaseRepository, ICurrencyCodeRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlCurrencyCodeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<bool> AddAsync(Currency entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Currency>> GetAllAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "Select * from dbo.VW_UNI_Currency_List";
                using var reader = await command.ExecuteReaderAsync();

                List<Currency> currencies = new List<Currency>();

                while (reader.Read())
                {
                    currencies.Add(reader.GetByEntityStructure<Currency>());
                }
                return currencies;
            }
        }

        public Task<Currency> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Currency>> GetCurrencyCodesByBusinessUnitId(string businessUnitCode)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = ReplaceQuery("[dbo].[VW_UNI_Currency_List]", new ReplaceParams { ParamName = "APT", Value = businessUnitCode });
                using var reader = await command.ExecuteReaderAsync();

                List<Currency> currencies = new List<Currency>();

                while (reader.Read())
                {
                    currencies.Add(reader.GetByEntityStructure<Currency>());
                }
                return currencies;
            }
        }

        public Task<bool> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Currency entity)
        {
            throw new NotImplementedException();
        }
    }
}
