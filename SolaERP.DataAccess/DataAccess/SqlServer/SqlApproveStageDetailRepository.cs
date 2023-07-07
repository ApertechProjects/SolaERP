using SolaERP.DataAccess.Extensions;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.ApproveStage;
using SolaERP.Application.UnitOfWork;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using SolaERP.Application.Models;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlApproveStageDetailRepository : IApproveStageDetailRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlApproveStageDetailRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ApproveStagesDetail>> GetByMainIdAsync(int approveStageMainId)
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
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = $"exec SP_ApproveStagesDetails_IUD @detailId,@NewApproveStageDetailsId = @NewApproveStageDetailsId OUTPUT select @NewApproveStageDetailsId as NewApproveStageDetailsId";
                IDbDataParameter dbDataParameter = command.CreateParameter();
                dbDataParameter.ParameterName = "@detailId";
                dbDataParameter.Value = id;
                command.Parameters.Add(dbDataParameter);
                command.Parameters.Add("@NewApproveStageDetailsId", SqlDbType.Int);
                command.Parameters["@NewApproveStageDetailsId"].Direction = ParameterDirection.Output;
                var value = await command.ExecuteNonQueryAsync();
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

        public async Task<int> SaveDetailsAsync(ApproveStageDetailInputModel entity)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "EXEC SP_ApproveStagesDetails_IUD @Id,@Id,@ApproveStageDetailsName,@Sequence,@Skip,@SkipDays,@BackToInitiatorOnReject," +
                    "@NewApproveStageDetailsId = @NewApproveStageDetailsId OUTPUT select @NewApproveStageDetailsId as NewApproveStageDetailsId";

                command.Parameters.AddWithValue(command, "@Id", entity.ApproveStageDetailsId = entity.ApproveStageDetailsId < 0 ? 0 : entity.ApproveStageDetailsId);
                command.Parameters.AddWithValue(command, "@Id", entity.ApproveStageMainId);
                command.Parameters.AddWithValue(command, "@ApproveStageDetailsName", entity.ApproveStageDetailsName);
                command.Parameters.AddWithValue(command, "@Sequence", entity.Sequence);
                command.Parameters.AddWithValue(command, "@Skip", entity.Skip);
                command.Parameters.AddWithValue(command, "@SkipDays", entity.SkipDays);
                command.Parameters.AddWithValue(command, "@BackToInitiatorOnReject", entity.BackToInitiatorOnReject);

                command.Parameters.Add("@NewApproveStageDetailsId", SqlDbType.Int);
                command.Parameters["@NewApproveStageDetailsId"].Direction = ParameterDirection.Output;

                using var reader = await command.ExecuteReaderAsync();
                int id = 0;
                if (reader.Read())
                {
                    id = reader.Get<int>("NewApproveStageDetailsId");
                }
                return id;
            }
        }
    }
}
