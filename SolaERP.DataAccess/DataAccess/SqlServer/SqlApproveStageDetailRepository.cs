using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.ApproveStage;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlApproveStageDetailRepository : IApproveStageDetailRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlApproveStageDetailRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<bool> AddAsync(ApproveStagesDetail entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<ApproveStagesDetail>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<ApproveStagesDetail>> GetApproveStageDetailsByApproveStageMainId(int approveStageMainId)
        {
            var result = await Task.Run(() =>
            {
                List<ApproveStagesDetail> approveStagesDetail = new List<ApproveStagesDetail>();

                using(var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = "EXEC dbo.SP_ApproveStageDetails_Load @approveStageMainId";
                    command.Parameters.AddWithValue(command, "@approveStageMainId", approveStageMainId);

                    using var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        approveStagesDetail.Add(reader.GetByEntityStructure<ApproveStagesDetail>());
                    }
                    return approveStagesDetail;
                }
            });
            return result;
        }

        public Task<ApproveStagesDetail> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(ApproveStagesDetail entity)
        {
            throw new NotImplementedException();
        }
    }
}
