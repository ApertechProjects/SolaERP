using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.AnalysisCode;
using SolaERP.Application.Entities.AnalysisCode;
using SolaERP.Application.Entities.AnalysisDimension;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data.Common;
using System.Data.SqlClient;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlAnalysisCodeRepository : IAnalysisStructureRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlAnalysisCodeRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> DeleteAnalysisCodeAsync(int analysisCodeId,int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_AnalysisCodes_IUD    
                                                                @AnalysisCodesId,
                                                                NULL,
                                                                NULL,
                                                                NULL,
                                                                NULL,
                                                                NULL,
                                                                NULL,
                                                                NULL,
                                                                NULL,
                                                                NULL,
                                                                NULL,
                                                                @UserId"
                ;

                command.Parameters.AddWithValue(command, "@AnalysisCodesId", analysisCodeId);
                command.Parameters.AddWithValue(command, "@UserId", userId);
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

        public async Task<List<Analysis>> GetAnalysisCodesAsync(int analysisCodeId, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_AnalysisCodes_Load @analysisCodeId,@userId";
                command.Parameters.AddWithValue(command, "@analysisCodeId", analysisCodeId);
                command.Parameters.AddWithValue(command, "@userId", userId);

                using var reader = await command.ExecuteReaderAsync();
                List<Analysis> resultList = new();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<Analysis>());

                return resultList;
            }
        }

        public async Task<List<AnalysisWithBu>> GetByBUIdAsync(int businessUnitId, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                List<AnalysisWithBu> resultList = new();

                command.CommandText = "EXEC [dbo].[SP_AnalysisCodes_Load_BY_BU] @businessUnitId,@userId";
                command.Parameters.AddWithValue(command, "@businessUnitId", businessUnitId);
                command.Parameters.AddWithValue(command, "@userId", userId);

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                    resultList.Add(reader.GetByEntityStructure<AnalysisWithBu>());

                return resultList;
            }
        }

        public async Task<List<AnalysisCodes>> GetAnalysisCodesByDimensionIdAsync(int dimensionId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_AnalysisCodes_Load_BY_DIM @DimensionId";
                command.Parameters.AddWithValue(command, "@DimensionId", dimensionId);

                using var reader = await command.ExecuteReaderAsync();
                List<AnalysisCodes> resultList = new();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<AnalysisCodes>());

                return resultList;
            }
        }

        public async Task<bool> SaveAnalysisCode(AnalysisCodeSaveModel analysisCodeSave, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_AnalysisCodes_IUD    
                                                                @AnalysisCodesId,
                                                                @AnalysisDimensionId,
                                                                @AnalysisCode,
                                                                @AnalysisName,
                                                                @Description,
                                                                @AdditionalDescription,
                                                                @AdditionalDescription2,
                                                                @Status,
                                                                @Date1,
                                                                @Date2,
                                                                @LinkedAnalysisDimensionid,
                                                                @UserId"
                ;

                command.Parameters.AddWithValue(command, "@AnalysisCodesId", analysisCodeSave.AnalysisCodesId);
                command.Parameters.AddWithValue(command, "@AnalysisDimensionId", analysisCodeSave.AnalysisDimensionId);
                command.Parameters.AddWithValue(command, "@AnalysisCode", analysisCodeSave.AnalysisCode);
                command.Parameters.AddWithValue(command, "@AnalysisName", analysisCodeSave.AnalysisName);
                command.Parameters.AddWithValue(command, "@Description", analysisCodeSave.Description);
                command.Parameters.AddWithValue(command, "@AdditionalDescription", analysisCodeSave.AdditionalDescription);
                command.Parameters.AddWithValue(command, "@AdditionalDescription2", analysisCodeSave.AdditionalDescription2);
                command.Parameters.AddWithValue(command, "@Status", analysisCodeSave.Status);
                command.Parameters.AddWithValue(command, "@Date1", analysisCodeSave.Date1);
                command.Parameters.AddWithValue(command, "@Date2", analysisCodeSave.Date2);
                command.Parameters.AddWithValue(command, "@LinkedAnalysisDimensionId", analysisCodeSave.LinkedAnalysisDimensionId);
                command.Parameters.AddWithValue(command, "@UserId", userId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }
}
