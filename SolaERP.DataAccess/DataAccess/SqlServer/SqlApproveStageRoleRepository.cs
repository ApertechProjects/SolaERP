using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.ApproveRole;
using SolaERP.Infrastructure.Entities.ApproveStage;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlApproveStageRoleRepository : IApproveStageRoleRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public Task<bool> AddAsync(ApproveStageRole entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddAsync(ApproveStageRole entity, int userId = 0)
        {
            string query = "exec exec SP_ApproveStageRoles_ID  @approveStageRoleId,@approveStageDetailId,@approveRoleId,@amountFrom,@amountTo";

            var result = await Task.Run(() =>
            {
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue(command, "@approveStageRoleId", entity.ApproveRoleId);
                    command.Parameters.AddWithValue(command, "@approveStageDetailId", entity.ApproveStageDetailId);
                    command.Parameters.AddWithValue(command, "@approveRoleId", entity.ApproveRoleId);
                    command.Parameters.AddWithValue(command, "@amountFrom", entity.AmountFrom);
                    command.Parameters.AddWithValue(command, "@amountTo", entity.AmountTo);
                    var value = command.ExecuteNonQuery();
                    return value == 0 || value == -1 ? false : true;
                }
            });

            return 0;
        }

        public Task<List<ApproveStageRole>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<ApproveStageRole>> GetApproveStageRolesByApproveStageDetailId(int approveStageDetailId)
        {
            var result = await Task.Run(() =>
            {
                List<ApproveStageRole> approveStageRoles = new List<ApproveStageRole>();
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = "EXEC dbo.SP_ApproveStageRoles_Load @approveStageDetailsId";
                    command.Parameters.AddWithValue(command, "@approveStageDetailsId", approveStageDetailId);
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        approveStageRoles.Add(GetFromReader(reader));
                    }
                    return approveStageRoles;
                }
            });
            return result;
        }

        public Task<ApproveStageRole> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int Id)
        {
            using (var commad = _unitOfWork.CreateCommand())
            {
                commad.CommandText = $"exec SP_ApproveStageRoles_ID @roleId";
                IDbDataParameter dbDataParameter = commad.CreateParameter();
                dbDataParameter.ParameterName = "@roleId";
                dbDataParameter.Value = Id;
                commad.Parameters.Add(dbDataParameter);
                var value = commad.ExecuteNonQuery();
                return value == 0 || value == -1 ? false : true;
            }
        }

        public void Update(ApproveStageRole entity)
        {
            throw new NotImplementedException();
        }

        private ApproveStageRole GetFromReader(IDataReader reader)
        {
            return new ApproveStageRole
            {
                ApproveStageRoleId = reader.Get<int>("ApproveStageRoleId"),
                ApproveStageDetailId = reader.Get<int>("ApproveStageDetailId"),
                ApproveRoleId = reader.Get<int>("ApproveRoleId"),
                ApproveRoles = new ApproveRole
                {
                    ApproveRoleId = reader.Get<int>("ApproveRoleId"),
                    ApproveRoleName = reader.Get<string>("ApproveRoleName")
                },
                AmountFrom = reader.Get<decimal>("AmountFrom"),
                AmountTo = reader.Get<decimal>("AmountTo")
            };
        }
    }
}
