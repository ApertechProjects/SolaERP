using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.AnaysisDimension;
using SolaERP.Application.Entities.AnalysisCode;
using SolaERP.Application.Entities.AnalysisDimension;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
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

        public async Task<List<BuAnalysisDimension>> ByBusinessUnitId(int businessUnitId, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                List<BuAnalysisDimension> resultList = new();

                command.CommandText = "EXEC [dbo].[SP_AnalysisDimension_Load_BY_BU] @businessUnitId,@userId";
                command.Parameters.AddWithValue(command, "@businessUnitId", businessUnitId);
                command.Parameters.AddWithValue(command, "@userId", userId);

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                    resultList.Add(reader.GetByEntityStructure<BuAnalysisDimension>());

                return resultList;
            }
        }

        public async Task<List<DimensionCheck>> CheckDimensionIdIsUsedInStructure(List<int> dimensionIds)
        {
            var dimensions = string.Join(',', dimensionIds);
            List<DimensionCheck> datas = new();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_CheckDimensionInStructure @DimensionIds";
                command.Parameters.AddWithValue(command, "@DimensionIds", dimensions);
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                    datas.Add(reader.GetByEntityStructure<DimensionCheck>());

                return datas;

            }
        }

        public async Task<bool> Delete(int analysisDimensionId, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_AnalysisDimension_IUD    
                                                                @AnalysisDimensionId,
                                                                NULL,
                                                                NULL,
                                                                NULL,
                                                                NULL,
                                                                NULL,
                                                                NULL,
                                                                NULL,
                                                                @UserId"
                ;

                command.Parameters.AddWithValue(command, "@AnalysisDimensionId", analysisDimensionId);
                command.Parameters.AddWithValue(command, "@UserId", userId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> Save(AnalysisDimensionDto analysisDimension, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_AnalysisDimension_IUD    
                                                                @AnalysisDimensionId,
                                                                @AnalysisDimensionCode,
                                                                @AnalysisDimensionName,
                                                                @BusinessUnitId,
                                                                @Description,
                                                                @Status,
                                                                @MinLength,
                                                                @MaxLength,
                                                                @UserId"
                ;

                command.Parameters.AddWithValue(command, "@AnalysisDimensionId", analysisDimension.AnalysisDimensionId);
                command.Parameters.AddWithValue(command, "@AnalysisDimensionCode", analysisDimension.AnalysisDimensionCode);
                command.Parameters.AddWithValue(command, "@AnalysisDimensionName", analysisDimension.AnalysisDimensionName);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", analysisDimension.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@Description", analysisDimension.Description);
                command.Parameters.AddWithValue(command, "@Status", analysisDimension.Status);
                command.Parameters.AddWithValue(command, "@MinLength", analysisDimension.MinLength);
                command.Parameters.AddWithValue(command, "@MaxLength", analysisDimension.MaxLength);
                command.Parameters.AddWithValue(command, "@UserId", userId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }
}
