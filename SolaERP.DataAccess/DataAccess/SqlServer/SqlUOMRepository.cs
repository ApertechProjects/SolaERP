using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.BusinessUnits;
using SolaERP.Infrastructure.Entities.UOM;
using SolaERP.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlUOMRepository : IUOMRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlUOMRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> AddAsync(UOM entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UOM>> GetAllAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "Select * from dbo.VW_UNI_UOM_List";
                using var reader = await command.ExecuteReaderAsync();

                List<UOM> UOMs = new List<UOM>();

                while (reader.Read())
                {
                    UOMs.Add(reader.GetByEntityStructure<UOM>());
                }
                return UOMs;
            }
        }

        public Task<UOM> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UOM entity)
        {
            throw new NotImplementedException();
        }
    }
}
