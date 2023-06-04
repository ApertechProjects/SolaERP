
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.AnalysisStructure;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlAnalysisStructureRepository : INewAnalysisStructureRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlAnalysisStructureRepository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;


        public async Task<bool> AddAsync(AnalysisStructureSaveModel model)
        {
            return await SaveAsync(0, model);
        }

        public async Task<AnalysisStructureWithBu> GetByBUAsync(int buId, int procedureId, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC [dbo].[SP_AnalysisStructure_Load_BY_BU] @BusinessUnitId,@UserId,@ProcedureId";
                command.Parameters.AddWithValue(command, "@BusinessUnitId", buId);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@ProcedureId", procedureId);

                AnalysisStructureWithBu resultEntity = default;
                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                    resultEntity = reader.GetByEntityStructure<AnalysisStructureWithBu>();

                return resultEntity;
            }
        }

        public async Task<AnalysisStructure> GetByIdAsync(int id)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC [dbo].[SP_AnalysisStructure_Load] @AnalysisStructureId,@UserId";
                command.Parameters.AddWithValue(command, "@AnalysisStructureId", id);
                command.Parameters.AddWithValue(command, "@UserId", 1);

                using var reader = await command.ExecuteReaderAsync();
                AnalysisStructure resultEntity = new();

                if (await reader.ReadAsync())
                    resultEntity = reader.GetByEntityStructure<AnalysisStructure>();

                return resultEntity;
            }
        }

        public async Task<bool> RemoveAsync(int id, int userId)
        {
            return await SaveAsync(id, new() { UserId = userId });
        }

        public async Task<bool> UpdateAsync(AnalysisStructureDeleteModel model)
        {
            return await SaveAsync(model.AnalysisStructureId, model);
        }

        private async Task<bool> SaveAsync(int mainId, AnalysisStructureSaveModel model)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC [dbo].[SP_AnalysisStructure_IUD] 
                                      @AnalysisStructureId,@BusinessUnitId,
                                      @ProcedureId,@CatId,@AnalysisDimensionid1,
                                      @AnalysisDimensionid2,@AnalysisDimensionid3,
                                      @AnalysisDimensionid4,@AnalysisDimensionid5,
                                      @AnalysisDimensionid6,@AnalysisDimensionid7,
                                      @AnalysisDimensionid8,@AnalysisDimensionid9,
                                      @AnalysisDimensionid10,@UserId";

                command.Parameters.AddWithValue(command, "@AnalysisStructureId", mainId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", model.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@ProcedureId", model.ProcedureId);
                command.Parameters.AddWithValue(command, "@CatId", model.ProcedureId);
                command.Parameters.AddWithValue(command, "@AnalysisDimensionid1", model.AnalysisDimensionid1);
                command.Parameters.AddWithValue(command, "@AnalysisDimensionid2", model.AnalysisDimensionid2);
                command.Parameters.AddWithValue(command, "@AnalysisDimensionid3", model.AnalysisDimensionid3);
                command.Parameters.AddWithValue(command, "@AnalysisDimensionid4", model.AnalysisDimensionid4);
                command.Parameters.AddWithValue(command, "@AnalysisDimensionid5", model.AnalysisDimensionid5);
                command.Parameters.AddWithValue(command, "@AnalysisDimensionid6", model.AnalysisDimensionid6);
                command.Parameters.AddWithValue(command, "@AnalysisDimensionid7", model.AnalysisDimensionid7);
                command.Parameters.AddWithValue(command, "@AnalysisDimensionid8", model.AnalysisDimensionid8);
                command.Parameters.AddWithValue(command, "@AnalysisDimensionid9", model.AnalysisDimensionid9);
                command.Parameters.AddWithValue(command, "@AnalysisDimensionid10", model.AnalysisDimensionid10);
                command.Parameters.AddWithValue(command, "@UserId", model.UserId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }
}
