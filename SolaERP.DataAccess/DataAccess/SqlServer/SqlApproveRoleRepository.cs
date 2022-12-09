using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.ApproveRole;
using SolaERP.Infrastructure.UnitOfWork;

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
            var result = await Task.Run(() =>
            {
                List<ApproveRole> approveRoles = new List<ApproveRole>();
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.VW_ApproveRoles_List";
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        approveRoles.Add(reader.GetByEntityStructure<ApproveRole>());
                    }

                    return approveRoles;
                }
            });
            return result;
        }

        public Task<ApproveRole> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(ApproveRole entity)
        {
            throw new NotImplementedException();
        }
    }
}
