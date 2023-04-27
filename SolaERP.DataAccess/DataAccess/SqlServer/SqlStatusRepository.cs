using SolaERP.DataAccess.Extensions;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.Entities.Status;
using SolaERP.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlStatusRepository : IStatusRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlStatusRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> AddAsync(Status entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Status>> GetAllAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "Select * from dbo.VW_Status_List";
                using var reader = await command.ExecuteReaderAsync();

                List<Status> status = new List<Status>();

                while (reader.Read())
                {
                    status.Add(reader.GetByEntityStructure<Status>());
                }
                return status;
            }
        }

        public Task<Status> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Status entity)
        {
            throw new NotImplementedException();
        }
    }
}
