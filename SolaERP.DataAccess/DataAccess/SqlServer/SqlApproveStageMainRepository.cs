using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.ApproveStage;
using SolaERP.Application.Entities.ApproveStages;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlApproveStageMainRepository : IApproveStageMainRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlApproveStageMainRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<int> AddAsync(ApprovalStagesMain entity, int userId = 0)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CheckVendorStage()
        {
            int count = 0;
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC [dbo].[SP_CheckStageVendor]";
                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    count = reader.Get<int>("Count");
                }
                return count > 0;
            }
        }

        public async Task<bool> DeleteApproveStageAsync(int approveStageMainId)
        {
            try
            {
                using (var command = _unitOfWork.CreateCommand() as SqlCommand)
                {
                    command.CommandText = $"SET NOCOUNT OFF exec SP_ApproveStagesMain_IUD @Id,@NewApproveStageMainId = @NewApproveStageMainId OUTPUT select @NewApproveStageMainId as NewApproveStageMainId";
                    IDbDataParameter dbDataParameter = command.CreateParameter();
                    dbDataParameter.ParameterName = "@Id";
                    dbDataParameter.Value = approveStageMainId;
                    command.Parameters.Add(dbDataParameter);
                    command.Parameters.Add("@NewApproveStageMainId", SqlDbType.Int);
                    command.Parameters["@NewApproveStageMainId"].Direction = ParameterDirection.Output;
                    var value = await command.ExecuteNonQueryAsync();
                    return value >= 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task<int> DeleteAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ApprovalStagesMain>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApprovalStagesMain> GetApprovalStageHeaderLoad(int approvalStageMainId)
        {
            ApprovalStagesMain approveStagesMain = null;
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

        public async Task<List<ApprovalStagesMain>> GetByBusinessUnitId(int buId)
        {
            List<ApprovalStagesMain> approveStagesMain = new List<ApprovalStagesMain>();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "Exec dbo.SP_ApproveStageMain_Load_BY_BU @BuId";
                command.Parameters.AddWithValue(command, "@BuId", buId);

                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    approveStagesMain.Add(reader.GetByEntityStructure<ApprovalStagesMain>("BusinessUnitId"));

                return approveStagesMain;
            }
        }

        public async Task<int> SaveApproveStageMainAsync(ApproveStageMainInputModel entity, int userId)
        {
            string query = "SET NOCOUNT OFF exec SP_ApproveStagesMain_IUD @approveStageMainId,@procedureId,@businessUnitId,@approveStageName,@approveStageCode,@reApproveOnChange,@userId," +
               "                                         @NewApproveStageMainId = @NewApproveStageMainId OUTPUT select @NewApproveStageMainId as NewApproveStageMainId";

            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = query;
                command.Parameters.AddWithValue(command, "@approveStageMainId", entity.ApproveStageMainId);
                command.Parameters.AddWithValue(command, "@procedureId", entity.ProcedureId);
                command.Parameters.AddWithValue(command, "@businessUnitId", entity.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@approveStageName", entity.ApproveStageName);
                command.Parameters.AddWithValue(command, "@approveStageCode", entity.ApproveStageCode);
                command.Parameters.AddWithValue(command, "@reApproveOnChange", entity.ReApproveOnChange);
                command.Parameters.AddWithValue(command, "@userId", userId);

                command.Parameters.Add("@NewApproveStageMainId", SqlDbType.Int);
                command.Parameters["@NewApproveStageMainId"].Direction = ParameterDirection.Output;

                using var reader = await command.ExecuteReaderAsync();
                int id = 0;
                if (reader.Read())
                {
                    id = reader.Get<int>("NewApproveStageMainId");
                }
                return id;
            }
        }

        public async Task<List<ApprovalStages>> Stages(int businessUnitId, string procedureKey)
        {
            List<ApprovalStages> approveStagesMain = new List<ApprovalStages>();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "Exec dbo.SP_ApproveStages_List @procedureKey, @businessUnitId";
                command.Parameters.AddWithValue(command, "@procedureKey", procedureKey);
                command.Parameters.AddWithValue(command, "@businessUnitId", businessUnitId);

                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    approveStagesMain.Add(reader.GetByEntityStructure<ApprovalStages>());

                return approveStagesMain;
            }
        }

        public Task<int> UpdateAsync(ApprovalStagesMain entity, int userId)
        {
            throw new NotImplementedException();
        }

        private ApprovalStagesMain GetFromReader(IDataReader reader)
        {
            return new ApprovalStagesMain
            {
                ApproveStageMainId = reader.Get<int>("ApproveStageMainId"),
                ApproveStageName = reader.Get<string>("ApproveStageName"),
                ProcedureId = reader.Get<int>("ProcedureId"),
                ProcedureName = reader.Get<string>("ProcedureName"),
                ApproveStageCode = reader.Get<string>("ApproveStageCode"),
                ReApproveOnChange = reader.Get<bool>("ReApproveOnChange"),
                BusinessUnitId = reader.Get<int>("BusinessUnitId")
            };
        }

    }
}
