using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Dtos.ApproveRole;
using SolaERP.Infrastructure.Entities.ApproveRole;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data.Common;
using System.Reflection;

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

        public async Task<bool> ApproveRoleSaveAsync(ApproveRole model)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF Exec SP_ApproveRoles_IUD  @ApproveRoleId,
                                                                        @ApproveRoleName";

                command.Parameters.AddWithValue(command, "@ApproveRoleId", model.ApproveRoleId);
                command.Parameters.AddWithValue(command, "@ApproveRoleName", model.ApproveRoleName);


                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> DeleteApproveRoleAsync(int approveRoleId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF Exec SP_ApproveRoles_IUD  @ApproveRoleId";
                command.Parameters.AddWithValue(command, "@ApproveRoleId", approveRoleId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<ApproveRole>> GetAllAsync()
        {
            List<ApproveRole> approveRoles = new List<ApproveRole>();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM dbo.VW_ApproveRoles_List";
                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    approveRoles.Add(reader.GetByEntityStructure<ApproveRole>());
                }

                return approveRoles;
            }
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
