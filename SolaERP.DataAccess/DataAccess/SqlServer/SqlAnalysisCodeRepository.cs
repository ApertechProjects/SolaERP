using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities;
using SolaERP.Infrastructure.Entities.AnalysisCode;
using SolaERP.Infrastructure.Entities.AnalysisDimension;
using SolaERP.Infrastructure.Entities.BusinessUnits;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data.Common;
using System.Reflection;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlAnalysisCodeRepository : IAnalysisCodeRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlAnalysisCodeRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> DeleteAnalysisCodeAsync(int groupAnalysisCodeId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF Exec SP_GroupAnalysisCodes_IUD  @GroupAnalysisCodeId";
                command.Parameters.AddWithValue(command, "@GroupAnalysisCodeId", groupAnalysisCodeId);
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<AnalysisCode>> GetAnalysisCodesAsync(int businessUnitId, string procedureName)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_AnalisisCodesList @BusinessUnitId,@ProcedureName";
                command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
                command.Parameters.AddWithValue(command, "@ProcedureName", procedureName);

                using var reader = await command.ExecuteReaderAsync();
                List<AnalysisCode> resultList = new();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<AnalysisCode>());

                return resultList;
            }
        }

        public async Task<List<GroupAnalysisCode>> GetAnalysisCodesByGroupIdAsync(int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_GroupAnalysisCodes_Load @GroupId";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);

                using var reader = await command.ExecuteReaderAsync();
                List<GroupAnalysisCode> resultList = new();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<GroupAnalysisCode>());

                return resultList;
            }
        }

        public async Task<List<AnalysisDimension>> GetAnalysisDimensionAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "select * from VW_AnalysisDimension_List";

                using var reader = await command.ExecuteReaderAsync();
                List<AnalysisDimension> resultList = new();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<AnalysisDimension>());

                return resultList;
            }
        }

        public async Task<bool> SaveAnalysisCodeAsync(AnalysisCodeSaveModel model)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF Exec SP_GroupAnalysisCodes_IUD  @GroupAnalysisCodeId,
                                                                       @GroupId,  
                                                                       @BusinessUnitId, 
                                                                       @AnalysisDimensionId, 
                                                                       @AnalysisCodesId";

                command.Parameters.AddWithValue(command, "@GroupAnalysisCodeId", model.GroupAnalysisCodeId);
                command.Parameters.AddWithValue(command, "@GroupId", model.GroupId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", model.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@AnalysisDimensionId", model.AnalysisDimensionId);
                command.Parameters.AddWithValue(command, "@AnalysisCodesId", model.AnalysisCodesId);


                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }
}
