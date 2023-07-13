using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.ApproveRole;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlApproveRoleRepository : IApproveRoleRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlApproveRoleRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> AddAsync(ApproveRole entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ApproveRole>> ApproveRoleAsync(int businessUnitId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec [dbo].[SP_ApproveRoles_Load] @businessUnitId";
                command.Parameters.AddWithValue(command, "@businessUnitId", businessUnitId);

                using var reader = await command.ExecuteReaderAsync();

                List<ApproveRole> roles = new List<ApproveRole>();

                while (reader.Read())
                {
                    roles.Add(reader.GetByEntityStructure<ApproveRole>());
                }
                return roles;
            }
        }

        public async Task<bool> ApproveRoleSaveAsync(ApproveRoleSaveModel model, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF Exec SP_ApproveRoles_IUD  @ApproveRoleId,
                                                                                  @ApproveRoleName,
                                                                                  @BusinessUnitId,
                                                                                  @UserId";

                command.Parameters.AddWithValue(command, "@ApproveRoleId", model.ApproveRoleId);
                command.Parameters.AddWithValue(command, "@ApproveRoleName", model.ApproveRoleName);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", model.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@UserId", userId);

                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<bool> ApproveRoleDeleteAsync(int approveRoleId, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF Exec SP_ApproveRoles_IUD  @approveRoleId,NULL,NULL,@userId";
                command.Parameters.AddWithValue(command, "@approveRoleId", approveRoleId);
                command.Parameters.AddWithValue(command, "@userId", userId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<ApproveRole>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApproveRole> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ApproveRole entity)
        {
            throw new NotImplementedException();
        }
    }
}
