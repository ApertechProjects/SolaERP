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

    }
}
