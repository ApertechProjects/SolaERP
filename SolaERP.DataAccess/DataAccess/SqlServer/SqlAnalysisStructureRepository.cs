
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.AnalysisStructure;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data.Common;
using System.Reflection;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlAnalysisStructureRepository : INewAnalysisStructureRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlAnalysisStructureRepository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;


        public async Task<List<AnalysisStructureWithBu>> GetByBUAsync(int buId, int procedureId, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC [dbo].[SP_AnalysisStructure_Load_BY_BU] @BusinessUnitId,@UserId,@ProcedureId";
                command.Parameters.AddWithValue(command, "@BusinessUnitId", buId);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@ProcedureId", procedureId);

                List<AnalysisStructureWithBu> resultEntity = new List<AnalysisStructureWithBu>();
                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                    resultEntity.Add(reader.GetByEntityStructure<AnalysisStructureWithBu>());

                return resultEntity;
            }
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC [dbo].[SP_AnalysisStructure_IUD] 
                                      @AnalysisStructureId,NULL,
                                      NULL,NULL,NULL,
                                      NULL,NULL,
                                      NULL,NULL,
                                      NULL,NULL,
                                      NULL,NULL,
                                      NULL,@UserId";

                command.Parameters.AddWithValue(command, "@AnalysisStructureId", id);
                command.Parameters.AddWithValue(command, "@UserId", userId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> SaveAsync(AnalysisStructureSaveModel model)
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

                command.Parameters.AddWithValue(command, "@AnalysisStructureId", model.AnalysisStructureId);
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
