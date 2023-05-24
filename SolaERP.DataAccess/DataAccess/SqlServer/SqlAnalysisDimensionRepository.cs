using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.AnalysisCode;
using SolaERP.Application.Entities.AnalysisDimension;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlAnalysisDimensionRepository : IAnalysisDimensionRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlAnalysisDimensionRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AnalysisDimension>> ByAnalysisDimensionId(int analysisDimensionId, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                List<AnalysisDimension> resultList = new();

                command.CommandText = "EXEC [dbo].[SP_AnalysisDimension_Load] @analysisDimensionId,@userId";
                command.Parameters.AddWithValue(command, "@analysisDimensionId", analysisDimensionId);
                command.Parameters.AddWithValue(command, "@userId", userId);

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                    resultList.Add(reader.GetByEntityStructure<AnalysisDimension>());

                return resultList;
            }
        }
    }
}
