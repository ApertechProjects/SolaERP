using System.Data;
using System.Data.Common;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.WellRepair;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;

namespace SolaERP.DataAccess.DataAccess.SqlServer;

public class SqlWellRepairRepository : IWellRepairRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public SqlWellRepairRepository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<List<WellRepairListDto>> GetWellRepairList() {
        using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"EXEC SP_WellRepairList";

        var data = new List<WellRepairListDto>();

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            data.Add(new WellRepairListDto
            {
                Status = reader.Get<int>("Status"),
                CreatedDate = reader.Get<DateTime>("CreatedDate"),
                CreatedBy = reader.Get<string>("CreatedBy"),
                RepairCode = reader.Get<string>("RepairCode"),
                RepairNameAz = reader.Get<string>("RepairNameAz"),
                RepairNameEng = reader.Get<string>("RepairNameEng"),
                WellRepairId = reader.Get<int>("WellRepairId")
            });
        }
        return data;
    }   
    public async Task<List<WellRepairLoadDto>> LoadWellRepairs(int wellRepairId) {
        
        using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"EXEC SP_WellRepairLoad @WellRepairId";
        command.Parameters.AddWithValue(command, "@WellRepairId", wellRepairId);

        var data = new List<WellRepairLoadDto>();

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            data.Add(new WellRepairLoadDto
            {
                Status = reader.Get<int>("Status"),
                CreatedDate = reader.Get<DateTime>("CreatedDate"),
                CreatedBy = reader.Get<int>("CreatedBy"),
                RepairCode = reader.Get<string>("RepairCode"),
                RepairNameAz = reader.Get<string>("RepairNameAz"),
                RepairNameEng = reader.Get<string>("RepairNameEng"),
                WellRepairId = reader.Get<int>("WellRepairId")
            });
        }
        return data;
    }

public async Task<List<WellCostListDto>> GetWellCostList(int businessUnitId, DateTime dateFrom, DateTime dateTo) 
{
    using var command = _unitOfWork.CreateCommand() as DbCommand;
    command.CommandText = @"EXEC SP_WellCostList  @BusinessUnitId, @DateFrom, @DateTo";
    command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
    command.Parameters.AddWithValue(command, "@DateFrom", dateFrom);
    command.Parameters.AddWithValue(command, "@DateTo", dateTo);

    var data = new List<WellCostListDto>();

    using var reader = await command.ExecuteReaderAsync();
    while (await reader.ReadAsync())
    {
        data.Add(new WellCostListDto
        {
            WellCostId = reader.Get<int>("WellCostId"),
            WellRepairId = reader.Get<int>("WellRepairId"),
            StartDate = reader.Get<DateTime>("StartDate"),
            EndDate = reader.Get<DateTime>("EndDate"),
            NumberOfStomp = reader.Get<int>("NumberOfStomp"),
            WellNumber = reader.Get<string>("WellNumber"),
            RepairNameAz = reader.Get<string>("RepairNameAz"),
            Description = reader.Get<string>("Description"),
            ProductionWay = reader.Get<string>("ProductionWay"),
            ProductionTon = reader.Get<decimal>("ProductionTon"),
            Subject = reader.Get<string>("Subject"),
            PeriodStart = reader.Get<DateTime>("PeriodStart"),
            PeriodEnd = reader.Get<DateTime>("PeriodEnd"),
            ActualPeriod = reader.Get<int>("ActualPeriod"),
            CreatedDate = reader.Get<DateTime>("CreatedDate"),
            CreatedBy = reader.Get<string>("CreatedBy"),
            FullName = reader.Get<string>("FullName"),
            BusinessUnitId = reader.Get<int>("BusinessUnitId")
        });
    }
    return data;
    }

public async Task<List<AnalysisFromSunListDto>> GetAnalysisListFromSun(int businessUnitId, int anlCatId)
{
    
    using var command = _unitOfWork.CreateCommand() as DbCommand;
    command.CommandText = @"EXEC SP_AnalysisListFromSun @BusinessUnitId, @ANL_CAT_ID";
    command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
    command.Parameters.AddWithValue(command, "@ANL_CAT_ID", anlCatId);

    var data = new List<AnalysisFromSunListDto>();

    using var reader = await command.ExecuteReaderAsync();
    while (await reader.ReadAsync())
        {
            data.Add(new AnalysisFromSunListDto
            {
                AnalysisName = reader.Get<string>("AnalysisName"),
                AnalysisCode = reader.Get<string>("AnalysisCode")
                
            });
        }
        return data;
    }

public async Task<bool> SaveWellRepairAsync(List<WellRepairRequest> dtoList,  int userId)
{
    bool result = false;
    foreach (var dto in dtoList){
        using (var command = _unitOfWork.CreateCommand() as DbCommand)
        {
            command.CommandText =
                @"SET NOCOUNT OFF Exec SP_WellRepair_IUD
                    @WellRepairId,
                    @RepairCode,
                    @RepairNameEng,
                    @RepairNameAz,
                    @Status,
                    @UserId,
                    @NewWellRepairId OUTPUT";

            command.Parameters.AddWithValue(command, "@WellRepairId", dto.WellRepairId);
            command.Parameters.AddWithValue(command, "@RepairCode", dto.RepairCode);
            command.Parameters.AddWithValue(command, "@RepairNameEng", dto.RepairNameEng);
            command.Parameters.AddWithValue(command, "@RepairNameAz", dto.RepairNameAz);
            command.Parameters.AddWithValue(command, "@Status", dto.Status);
            command.Parameters.AddWithValue(command, "@UserId", userId);
            
            var outputId = command.CreateParameter();
            outputId.ParameterName = "@NewWellRepairId";
            outputId.DbType = DbType.Int32;
            outputId.Direction = ParameterDirection.Output;
            command.Parameters.Add(outputId);

            if (await command.ExecuteNonQueryAsync() > 0)
                result = true;
        }
        
    }
        return result;
    }

public async Task<bool> SaveWellCostAsync(List<WellCostRequest> dtoList,  int userId)
{
    bool result = false;
    foreach (var dto in dtoList)
    {
        using (var command = _unitOfWork.CreateCommand() as DbCommand)
        {
            command.CommandText =
                @"SET NOCOUNT OFF; 
              EXEC SP_WellCost_IUD
                @WellCostId,
                @BusinessUnitId,
                @WellNumber,
                @StartDate,
                @EndDate,
                @NumberOfStomp,
                @WellRepairId,
                @Description,
                @ProductionWay,
                @ProductionTon,
                @Subject,
                @PeriodStart,
                @PeriodEnd,
                @ActualPeriod,
                @UserId,
                @NewWellCostId OUTPUT";

            command.Parameters.AddWithValue(command, "@WellCostId", dto.WellCostId);
            command.Parameters.AddWithValue(command, "@BusinessUnitId", dto.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@WellNumber", dto.WellNumber);
            command.Parameters.AddWithValue(command, "@StartDate", dto.StartDate);
            command.Parameters.AddWithValue(command, "@EndDate", dto.EndDate);
            command.Parameters.AddWithValue(command, "@NumberOfStomp", dto.NumberOfStomp);
            command.Parameters.AddWithValue(command, "@WellRepairId", dto.WellRepairId);
            command.Parameters.AddWithValue(command, "@Description", dto.Description);
            command.Parameters.AddWithValue(command, "@ProductionWay", dto.ProductionWay);
            command.Parameters.AddWithValue(command, "@ProductionTon", dto.ProductionTon);
            command.Parameters.AddWithValue(command, "@Subject", dto.Subject);
            command.Parameters.AddWithValue(command, "@PeriodStart", dto.PeriodStart);
            command.Parameters.AddWithValue(command, "@PeriodEnd", dto.PeriodEnd);
            command.Parameters.AddWithValue(command, "@ActualPeriod", dto.ActualPeriod);
            command.Parameters.AddWithValue(command, "@UserId", userId);

            var outputId = command.CreateParameter();
            outputId.ParameterName = "@NewWellCostId";
            outputId.DbType = DbType.Int32;
            outputId.Direction = ParameterDirection.Output;
            command.Parameters.Add(outputId);

            if (await command.ExecuteNonQueryAsync() > 0)
                result = true;
        }
    }

    return result;
}

}













