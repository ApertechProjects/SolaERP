using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.ApproveStage;
using SolaERP.Infrastructure.Entities.Procedure;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlApproveStageMainRepository : IApproveStageMainRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlApproveStageMainRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> AddAsync(ApproveStagesMain entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<ApproveStagesMain>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApproveStagesMain> GetApprovalStageHeaderLoad(int approvalStageMainId)
        {
            var result = await Task.Run(() =>
            {
                ApproveStagesMain approveStagesMain = null;
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = "EXEC dbo.SP_ApproveStageHeader_Load_BY_MainId @approvalStageMainId";
                    command.Parameters.AddWithValue(command, "@approvalStageMainId", approvalStageMainId);
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        approveStagesMain = GetFromReader(reader);
                    }
                    return approveStagesMain;
                }
            });
            return result;
        }

        public async Task<List<ApproveStagesMain>> GetByBusinessUnitId(int buId)
        {
            var result = await Task.Run(() =>
            {
                List<ApproveStagesMain> approveStagesMain = new List<ApproveStagesMain>();
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = "Exec dbo.SP_ApproveStageMain_Load_BY_BU @BuId";
                    command.Parameters.AddWithValue(command, "@BuId", buId);

                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        approveStagesMain.Add(GetFromReader(reader));
                    }
                    return approveStagesMain;
                }
            });
            return result;
        }

        public Task<ApproveStagesMain> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(ApproveStagesMain entity)
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
                    ProcedureName = reader.Get<string>("ProcedureName")
                }
            };
        }

    }
}
