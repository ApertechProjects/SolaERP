using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlAnalysisCodeRepository : IAnalysisCodeRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlAnalysisCodeRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AnalysisCode>> GetAnalysisCodesAsync(int businessUnitId, string procedureName, int sequence)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_AnalisisCodesList @BusinessUnitId,@ProcedureName,@Sequence";
                command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
                command.Parameters.AddWithValue(command, "@ProcedureName", procedureName);
                command.Parameters.AddWithValue(command, "@Sequence", sequence);

                using var reader = await command.ExecuteReaderAsync();
                List<AnalysisCode> resultList = new();

                while (await reader.ReadAsync()) resultList.Add(reader.GetByEntityStructure<AnalysisCode>());
                return resultList;
            }
        }
    }
}
