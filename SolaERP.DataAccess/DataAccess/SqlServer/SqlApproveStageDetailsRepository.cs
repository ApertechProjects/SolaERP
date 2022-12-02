using SolaERP.Infrastructure.Entities.ApproveStage;
using SolaERP.Infrastructure.Repositories;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlApproveStageDetailsRepository : IApprovelStageDetailsRepository
    {
        public Task<bool> AddAsync(ApproveStagesDetail entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<ApproveStagesDetail>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApproveStagesDetail> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(ApproveStagesDetail entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ApproveStagesDetail entity)
        {
            throw new NotImplementedException();
        }
    }
}
