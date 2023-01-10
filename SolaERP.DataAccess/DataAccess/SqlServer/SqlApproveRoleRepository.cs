using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.ApproveRole;
using SolaERP.Infrastructure.UnitOfWork;
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
