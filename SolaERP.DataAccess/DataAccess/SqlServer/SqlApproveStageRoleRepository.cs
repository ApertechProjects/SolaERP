using SolaERP.DataAccess.Extensions;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.ApproveRole;
using SolaERP.Application.Entities.ApproveStage;
using SolaERP.Application.UnitOfWork;
using System.Data;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlApproveStageRoleRepository : IApproveStageRoleRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlApproveStageRoleRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ApprovalStageRole>> GetByDetailIdAsync(int approveStageDetailId)
        {
            List<ApprovalStageRole> approveStageRoles = new List<ApprovalStageRole>();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC dbo.SP_ApproveStageRoles_Load @approveStageDetailsId";
                command.Parameters.AddWithValue(command, "@approveStageDetailsId", approveStageDetailId);
                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    approveStageRoles.Add(GetFromReader(reader));
                }
                return approveStageRoles;
            }
        }

        public Task<ApprovalStageRole> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveAsync(int Id)
        {
            using (var commad = _unitOfWork.CreateCommand() as DbCommand)
            {
                commad.CommandText = $"SET NOCOUNT OFF exec SP_ApproveStageRoles_IUD @roleId";
                IDbDataParameter dbDataParameter = commad.CreateParameter();
                dbDataParameter.ParameterName = "@roleId";
                dbDataParameter.Value = Id;
                commad.Parameters.Add(dbDataParameter);
                var value = await commad.ExecuteNonQueryAsync();
                return value > 0;
            }
        }

        public Task<int> UpdateAsync(ApprovalStageRole entity, int userId = 0)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddAsync(ApprovalStageRole entity)
        {
            string query = "SET NOCOUNT OFF exec SP_ApproveStageRoles_IUD  @approveStageRoleId,@approveStageDetailId,@approveRoleId,@amountFrom,@amountTo";

            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = query;
                command.Parameters.AddWithValue(command, "@approveStageRoleId", entity.Id = entity.Id < 0 ? 0 : entity.Id);
                command.Parameters.AddWithValue(command, "@approveStageDetailId", entity.ApproveStageDetailId);
                command.Parameters.AddWithValue(command, "@approveRoleId", entity.ApproveRoleId);
                command.Parameters.AddWithValue(command, "@amountFrom", entity.AmountFrom);
                command.Parameters.AddWithValue(command, "@amountTo", entity.AmountTo);
                var value = await command.ExecuteNonQueryAsync();
                return value;
            }
        }

        private ApprovalStageRole GetFromReader(IDataReader reader)
        {
            return new ApprovalStageRole
            {
                Id = reader.Get<int>("Id"),
                ApproveStageDetailId = reader.Get<int>("ApproveStageDetailId"),
                ApproveRoleId = reader.Get<int>("ApproveRoleId"),
                ApproveRoleName = reader.Get<string>("ApproveRoleName"),
                AmountFrom = reader.Get<decimal>("AmountFrom"),
                AmountTo = reader.Get<decimal>("AmountTo")
            };
        }
    }
}
