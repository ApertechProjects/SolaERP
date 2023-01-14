using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.ApproveStage;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlApproveStageDetailRepository : IApproveStageDetailRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlApproveStageDetailRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

                using (var command = _unitOfWork.CreateCommand())
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

        public async Task<bool> RemoveAsync(int id)
        {
            using (var commad = _unitOfWork.CreateCommand() as DbCommand)
            {
                commad.CommandText = $"exec SP_ApproveStagesDetails_IUD @detailId";
                IDbDataParameter dbDataParameter = commad.CreateParameter();
                dbDataParameter.ParameterName = "@detailId";
                dbDataParameter.Value = id;
                commad.Parameters.Add(dbDataParameter);
                var value = await commad.ExecuteNonQueryAsync();
                return value >= 0;
            }
        }

        public Task UpdateAsync(ApproveStagesDetail entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddRangeAsync(List<ApproveStagesDetail> entities)
        {
            string query = "exec SP_ApproveStagesDetails_IUD @approveStageDetailId,@approveStageMainId,@approveStageDetailName,@sequence";

            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = query;
                foreach (var entity in entities)
                {
                    command.Parameters.AddWithValue(command, "@approveStageDetailId", entity.ApproveStageDetailsId);
                    command.Parameters.AddWithValue(command, "@approveStageMainId", entity.ApproveStageMainId);
                    command.Parameters.AddWithValue(command, "@approveStageDetailName", entity.ApproveStageDetailsName);
                    command.Parameters.AddWithValue(command, "@sequence", entity.Sequence);
                }
                return await command.ExecuteNonQueryAsync();
            }
        }

        Task<int> IReturnableAddAsync<ApproveStagesDetail>.AddAsync(ApproveStagesDetail entity)
        {
            throw new NotImplementedException();
        }
    }
}
