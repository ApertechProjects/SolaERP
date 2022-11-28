using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Entities.Groups;
using SolaERP.Infrastructure.Repositories;
using SolaERP.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var result = await Task.Run(() =>
            {
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = "EXEC dbo.SP_GroupMain_Load";
                    using var reader = command.ExecuteReader();

                    List<Groups> groups = new List<Groups>();
                    while (reader.Read())
                    {
                        groups.Add(reader.GetByEntityStructure<Groups>());
                    }
                    return groups;
                }
            });
            return result;
        }

        public Task<Groups> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(Groups entity)
        {
            using (var command = _unitOfWork.CreateCommand())
            {
                command.CommandText = "Delete from Config.AppUser Where Id = @Id";
                IDbDataParameter dbDataParameter = command.CreateParameter();
                dbDataParameter.ParameterName = "@Id";
                dbDataParameter.Value = entity.GroupId;
                command.Parameters.Add(dbDataParameter);

                command.ExecuteNonQuery();
            }
        }

        public void Update(Groups entity)
        {
            using (var command = _unitOfWork.CreateCommand())
            {
                command.CommandText = "EXEC SP_Groups_IUD @groupId";
                IDbDataParameter dbDataParameter = command.CreateParameter();
                dbDataParameter.ParameterName = "@groupId";
                dbDataParameter.Value = entity.GroupId;
                command.Parameters.Add(dbDataParameter);
                command.ExecuteNonQuery();
            }
        }
    }
}
