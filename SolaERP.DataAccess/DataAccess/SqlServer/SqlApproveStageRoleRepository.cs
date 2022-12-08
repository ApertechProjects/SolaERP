using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.ApproveRole;
using SolaERP.Infrastructure.Entities.ApproveStage;
using SolaERP.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlApproveStageRoleRepository : IApproveStageRoleRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public Task<bool> AddAsync(ApproveStageRole entity)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
