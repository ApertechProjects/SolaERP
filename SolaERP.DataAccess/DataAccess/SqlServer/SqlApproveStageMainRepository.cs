using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.ApproveStage;
using SolaERP.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<ApproveStagesMain>> GetByBusinessUnitId(int buId)
        {
            //bu duz gelmir cunki,ApproveStageMainde meselen Id ler var, gelen prosedurda ise Id ye qarshiliq
            //gelen datalar bu shekilde duzgun oturmur,prosedura Id elave etsek bele, meselen
            //ProcedureId ApproveStageMainde var amma prosedurda yoxdu ona gore uygun datani set ede bilmir
            var result = await Task.Run(() =>
            {
                List<ApproveStagesMain> approveStagesMain = new List<ApproveStagesMain>();
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = "Exec dbo.SP_ApproveStageMain_Load @BuId";
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
                ProcedureId = reader.Get<int>("ProcedureId")
            };
        }
    }
}
