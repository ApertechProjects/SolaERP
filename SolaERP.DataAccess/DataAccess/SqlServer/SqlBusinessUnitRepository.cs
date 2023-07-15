using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlBusinessUnitRepository : IBusinessUnitRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlBusinessUnitRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region DML Operations

        public async Task<bool> AddAsync(BusinessUnits entity)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SP_BusinessUnits_IUD";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue(command, "@BusinessUnitId", null);
                command.Parameters.AddWithValue(command, "@BusinessUnitCode", entity.BusinessUnitCode);
                command.Parameters.AddWithValue(command, "@BusinessUnitName", entity.BusinessUnitName);

                var result = await command.ExecuteNonQueryAsync();
                return result > 0 ? true : false;
            }
        }
        public async Task<bool> RemoveAsync(int Id)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SP_BusinessUnits_IUD";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue(command, "@BusinessUnitId", Id);
                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }

        }
        public async Task UpdateAsync(BusinessUnits entity)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SP_BusinessUnits_IUD";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue(command, "@BusinessUnitId", entity.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@BusinessUnitCode", entity.BusinessUnitCode);
                command.Parameters.AddWithValue(command, "@BusinessUnitName", entity.BusinessUnitName);

                await command.ExecuteNonQueryAsync();
            }
        }

        #endregion
        //
        #region Get Operations

        public async Task<List<BusinessUnits>> GetAllAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "Select * from dbo.VW_BusinessUnits_List";
                using var reader = await command.ExecuteReaderAsync();

                List<BusinessUnits> businessUnits = new List<BusinessUnits>();

                while (reader.Read())
                {
                    businessUnits.Add(reader.GetByEntityStructure<BusinessUnits>());
                }
                return businessUnits;
            }
        }
        public async Task<List<BaseBusinessUnit>> GetBusinessUnitListByUserId(int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = $"EXEC SP_BusinessUnitsList {userId}";
                using var reader = await command.ExecuteReaderAsync();

                List<BaseBusinessUnit> businessUnits = new List<BaseBusinessUnit>();

                while (reader.Read())
                    businessUnits.Add(reader.GetByEntityStructure<BaseBusinessUnit>());

                return businessUnits;
            }
        }
        public async Task<BusinessUnits> GetByIdAsync(int id)
        {
            BusinessUnits result = null;
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM FN_GetBusinessUnit_By_Id(@BusinessUnitId)";
                command.Parameters.AddWithValue(command, "@BusinessUnitId", id);

                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                    result = reader.GetByEntityStructure<BusinessUnits>();

                return result;
            }
        }

        #endregion

    }
}
