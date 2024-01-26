using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.UOM;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlUOMRepository : SqlBaseRepository, IUOMRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlUOMRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
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

        public async Task<List<UOM>> GetUOMListBusinessUnitCode(int businessUnitId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec dbo.SP_UOMList @businessUnitId";
                command.Parameters.AddWithValue(command, "@businessUnitId", businessUnitId);
                using var reader = await command.ExecuteReaderAsync();

                List<UOM> UOMs = new List<UOM>();

                while (reader.Read())
                {
                    UOMs.Add(reader.GetByEntityStructure<UOM>());
                }
                return UOMs;
            }
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
