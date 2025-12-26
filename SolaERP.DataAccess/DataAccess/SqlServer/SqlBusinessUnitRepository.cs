using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

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
                command.CommandText = "SELECT * FROM Config.BusinessUnits";
                using var reader = await command.ExecuteReaderAsync();

                List<BusinessUnits> businessUnits = new List<BusinessUnits>();

                while (reader.Read())
                {
                    businessUnits.Add(reader.GetByEntityStructure<BusinessUnits>("ExportOilPercent"));
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

        public async Task<List<BaseBusinessUnit>> GetBusinessUnitListByCurrentUser(int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = $"EXEC SP_UserBusinessUnitsList {userId}";
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
                    result = new BusinessUnits()
                    {
                        BusinessUnitCode = reader.Get<string>("BusinessUnitCode"),
                        BusinessUnitName = reader.Get<string>("BusinessUnitName"),
                        BusinessUnitId = reader.Get<int>("BusinessUnitId"),
                        TaxId = reader.Get<string>("TaxId"),
                        Address = reader.Get<string>("Address"),
                        Position = reader.Get<string>("Position"),
                        CountryCode = reader.Get<string>("CountryCode"),
                        FullName = reader.Get<string>("FullName"),
                        UseOrderForInvoice = reader.Get<bool>("UseOrderForInvoice")
					};

                return result;
            }
        }

        public async Task<string> GetBusinessUnitCode(int businessUnitId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = $"select BusinessUnitCode from Config.BusinessUnits where BusinessUnitId = @businessUnitId";
                command.Parameters.AddWithValue(command, "@businessUnitId", businessUnitId);
                using var reader = await command.ExecuteReaderAsync();

                string result = string.Empty;

                if (reader.Read())
                    result = reader.Get<string>("BusinessUnitCode");

                return result;
            }
        }

        public async Task<string> GetBusinessUnitName(int businessUnitId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = $"select BusinessUnitName from Config.BusinessUnits where BusinessUnitId = @businessUnitId";
                command.Parameters.AddWithValue(command, "@businessUnitId", businessUnitId);
                using var reader = await command.ExecuteReaderAsync();

                string result = string.Empty;

                if (reader.Read())
                    result = reader.Get<string>("BusinessUnitName");

                return result;
            }
        }

        #endregion

    }
}
