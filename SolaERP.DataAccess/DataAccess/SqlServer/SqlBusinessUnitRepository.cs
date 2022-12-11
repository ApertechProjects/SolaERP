using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.BusinessUnits;
using SolaERP.Infrastructure.UnitOfWork;
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
        public bool Remove(int Id)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SP_BusinessUnits_IUD";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue(command, "@BusinessUnitId", Id);

                var result = command.ExecuteNonQuery();

                return result > 0;
            }

        }
        public async void Update(BusinessUnits entity)
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

        public Task<List<BusinessUnits>> GetAllAsync()
        {
            var result = Task.Run(() =>
            {
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = "Select * from dbo.VW_BusinessUnits_List";
                    using var reader = command.ExecuteReader();

                    List<BusinessUnits> businessUnits = new List<BusinessUnits>();

                    while (reader.Read())
                    {
                        businessUnits.Add(reader.GetByEntityStructure<BusinessUnits>());
                    }
                    return businessUnits;
                }
            });
            return result;
        }
        public Task<List<BusinessUnits>> GetBusinessUnitListByUserId(int userId)
        {
            var result = Task.Run(() =>
            {
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = $"EXEC SP_BusinessUnitsList {userId}";
                    using var reader = command.ExecuteReader();

                    List<BusinessUnits> businessUnits = new List<BusinessUnits>();

                    while (reader.Read())
                    {
                        businessUnits.Add(reader.GetByEntityStructure<BusinessUnits>());
                    }
                    return businessUnits;
                }
            });
            return result;
        }
        public async Task<BusinessUnits> GetByIdAsync(int id)
        {
            BusinessUnits result = null;
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM FN_GetBusinessUnit_By_Id(@BusinessUnitId)";
                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                    result = reader.GetByEntityStructure<BusinessUnits>();

                return result;
            }
        }
        public async Task<List<BusinessUnitForGroup>> GetBusinessUnitForGroups(int groupId)
        {
            List<BusinessUnitForGroup> businessUnitForGroups = new();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SP_GroupBusinessUnit_Load";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(command, "@GroupId", groupId);

                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    businessUnitForGroups.Add(reader.GetByEntityStructure<BusinessUnitForGroup>());

                return businessUnitForGroups;
            }
        }

        #endregion

    }
}
