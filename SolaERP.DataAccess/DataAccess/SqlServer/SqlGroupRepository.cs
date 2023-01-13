using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Groups;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlGroupRepository : IGroupRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlGroupRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> AddAsync(Groups entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Groups>> GetAllAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC dbo.SP_GroupMain_Load";
                using var reader = await command.ExecuteReaderAsync();

                List<Groups> groups = new List<Groups>();
                while (reader.Read())
                {
                    groups.Add(reader.GetByEntityStructure<Groups>());
                }
                return groups;
            }
        }

        public Task<Groups> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveAsync(int Id)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "Delete from Config.AppUser Where Id = @Id";
                IDbDataParameter dbDataParameter = command.CreateParameter();
                dbDataParameter.ParameterName = "@Id";
                dbDataParameter.Value = Id;
                command.Parameters.Add(dbDataParameter);
                var value = await command.ExecuteNonQueryAsync();
                return value > 0;
            }
        }

        public async Task UpdateAsync(Groups entity)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_Groups_IUD @groupId";
                IDbDataParameter dbDataParameter = command.CreateParameter();
                dbDataParameter.ParameterName = "@groupId";
                dbDataParameter.Value = entity.GroupId;
                command.Parameters.Add(dbDataParameter);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
