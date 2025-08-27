using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.UnitOfWork;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using SolaERP.Application.Dtos.BarrelFlow;
using SolaERP.DataAccess.Extensions;

namespace SolaERP.DataAccess.DataAccess.SqlServer;

public class SqlBarrelFlowRepository : IBarrelFlowRepository
{
    private readonly IUnitOfWork _unitOfWork;
    public SqlBarrelFlowRepository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> SaveBarrelFlowsRegisterIUD(DataTable dataTable)
    {
        await using var command = _unitOfWork.CreateCommand() as SqlCommand;
        command.CommandText = @"EXEC SP_BarrelFlowRegister_IUD @data";

        command.Parameters.AddTableValue(command, "@data", "BarrelFlowRegisterType", dataTable);
        var value = await command.ExecuteNonQueryAsync();

        return value > 0;
    }

    public async Task<List<BarrelFlowRegisterDto>> GetBarrelFlowRegister(int businessUnitId, DateTime dateFrom,
        DateTime dateTo)
    {
        try
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"EXEC SP_BarrelFlowRegister @BusinessUnitId, @DateFrom, @DateTo";

            command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
            command.Parameters.AddWithValue(command, "@DateFrom", dateFrom);
            command.Parameters.AddWithValue(command, "@DateTo", dateTo);

            var data = new List<BarrelFlowRegisterDto>();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                data.Add(new BarrelFlowRegisterDto
                {
                    BarrelFlowRegisterId = reader.Get<int>("BarrelFlowRegisterId"),
                    Period = reader.Get<int>("Period"),
                    Date = reader.Get<DateTime>("Date"),
                    OpeningOilStockTon = reader.Get<decimal>("OpeningOilStockTon"),
                    BarrelFactor = reader.Get<decimal>("BarrelFactor"),
                    ProductionTon = reader.Get<decimal>("ProductionTon"),
                    OtherUsage = reader.Get<decimal>("OtherUsage"),
                    Delivery = reader.Get<decimal>("Delivery"),
                    ExportOilPercent = reader.Get<decimal>("ExportOilPercent"),
                    ExportOil = reader.Get<decimal>("ExportOil"),
                    COPercent = reader.Get<decimal>("COPercent"),
                    CO = reader.Get<decimal>("CO"),
                    ClosingOilStockTon = reader.Get<decimal>("ClosingOilStockTon"),
                    CrudeOilTon = reader.Get<decimal>("CrudeOilTon"),
                    ProcessOilTon = reader.Get<decimal>("ProcessOilTon"),
                    PipelinesOilTon = reader.Get<decimal>("PipelinesOilTon"),
                    CreatedBy = reader.Get<int>("CreatedBy"),
                    FullName = reader.Get<string>("FullName"),
                    CreatedDate = reader.Get<DateTime>("CreatedDate"),
                    BusinessUnitId = reader.Get<int>("BusinessUnitId"),
                    Status = reader.Get<string>("Status")
                });
            }

            return data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}