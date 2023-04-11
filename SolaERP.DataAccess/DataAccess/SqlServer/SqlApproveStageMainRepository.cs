using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.ApproveStage;
using SolaERP.Infrastructure.Entities.ApproveStages;
using SolaERP.Infrastructure.Entities.Procedure;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlApproveStageMainRepository : IApproveStageMainRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlApproveStageMainRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<int> AddAsync(ApproveStagesMain entity, int userId = 0)
        {
            string query = "exec SP_ApproveStagesMain_IUD @approveStageMainId,@procedureId,@businessUnitId,@approveStageName,@userId";

            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = query;
                command.Parameters.AddWithValue(command, "@approveStageMainId", entity.ApproveStageMainId);
                command.Parameters.AddWithValue(command, "@procedureId", entity.ProcedureId);
                command.Parameters.AddWithValue(command, "@businessUnitId", entity.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@approveStageName", entity.ApproveStageName);
                command.Parameters.AddWithValue(command, "@userId", userId);
                command.Parameters.AddOutPutParameter(command, "@NewApproveStageMainId");

                int result = 0;
                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync()) result = reader.Get<int>("NewApproveStageMainId");

                return result;
            }
        }

        public Task<int> DeleteAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ApproveStagesMain>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApproveStagesMain> GetApprovalStageHeaderLoad(int approvalStageMainId)
        {
            ApproveStagesMain approveStagesMain = null;
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC dbo.SP_ApproveStageHeader_Load_BY_MainId @approvalStageMainId";
                command.Parameters.AddWithValue(command, "@approvalStageMainId", approvalStageMainId);
                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    approveStagesMain = GetFromReader(reader);
                }
                return approveStagesMain;
            }
        }

        public async Task<List<ApprovalStatus>> GetApprovalStatusList()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM VW_ApprovalStatus_List";
                using var reader = await command.ExecuteReaderAsync();

                List<ApprovalStatus> approvalStatuses = new();
                while (await reader.ReadAsync())
                {
                    approvalStatuses.Add(reader.GetByEntityStructure<ApprovalStatus>());
                }
                return approvalStatuses;
            }

        }

        public async Task<List<ApproveStagesMain>> GetByBusinessUnitId(int buId)
        {
            List<ApproveStagesMain> approveStagesMain = new List<ApproveStagesMain>();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "Exec dbo.SP_ApproveStageMain_Load_BY_BU @BuId";
                command.Parameters.AddWithValue(command, "@BuId", buId);

                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    approveStagesMain.Add(reader.GetByEntityStructure<ApproveStagesMain>());
                }
                return approveStagesMain;
            }
        }


        public Task<int> UpdateAsync(ApproveStagesMain entity, int userId)
        {
            throw new NotImplementedException();
        }

        private ApproveStagesMain GetFromReader(IDataReader reader)
        {
            return new ApproveStagesMain
            {
                ApproveStageMainId = reader.Get<int>("ApproveStageMainId"),
                ApproveStageName = reader.Get<string>("ApproveStageName"),
                BusinessUnitId = reader.Get<int>("BusinessUnitId"),
                ProcedureId = reader.Get<int>("ProcedureId"),
                Procedure = new Procedure
                {
                    ProcedureId = reader.Get<int>("ProcedureId"),
                    ProcedureName = reader.Get<string>("ProcedureName"),
                }
            };
        }

    }
}
