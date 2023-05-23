using SolaERP.Application.Entities.AnalysisCode;
using SolaERP.DataAccess.Extensions;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.AnalysisDimension;
using SolaERP.Application.UnitOfWork;
using System.Data.Common;
using SolaERP.Application.Entities.BusinessUnits;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlAnalysisCodeRepository : IAnalysisCodeRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlAnalysisCodeRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        public async Task<List<BuAnalysisDimension>> GetBusinessUnitDimensions(int businessUnitId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                List<BuAnalysisDimension> resultList = new();

                command.CommandText = "EXEC [dbo].[SP_AnalysisDimension_Load_BY_BU] @businessUniId";
                command.Parameters.AddWithValue(command, "@businessUniId", businessUnitId);


                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                    resultList.Add(reader.GetByEntityStructure<BuAnalysisDimension>());

                return resultList;
            }
        }
    }
}
