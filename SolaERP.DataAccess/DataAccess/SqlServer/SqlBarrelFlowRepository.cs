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
                Status = reader.Get<string>("Status"),
                OpeningOilStockBBL = reader.Get<decimal>("OpeningOilStockBBL"),
                OBConsignmentBBL = reader.Get<decimal>("OBConsignmentBBL"),
                ClosingOilStockBBL = reader.Get<decimal>("ClosingOilStockBBL"),
                CBConsignmentBBL = reader.Get<decimal>("CBConsignmentBBL"),
                RealizedSales = reader.Get<decimal>("RealizedSales")
            });
        }

        return data;
    }

    public async Task<List<BarrelFlowPeriodListDto>> GetPeriodListByBusinessId(int businessUnitId)
    {
        using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"EXEC SP_PeriodListByBusinessId @BusinessUnitId";

        command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);

        var data = new List<BarrelFlowPeriodListDto>();

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            data.Add(new BarrelFlowPeriodListDto
            {
                PeriodFrom = reader.Get<string>("periodFrom"),
                PeriodTo = reader.Get<string>("periodTo")
            });
        }

        return data;
    }

    public async Task<bool> SaveBarrelFlowBudgetRegisterIUD(DataTable dataTable)
    {
        await using var command = _unitOfWork.CreateCommand() as SqlCommand;
        command.CommandText = @"EXEC SP_BarrelFlowBudgetRegister_IUD @data";

        command.Parameters.AddTableValue(command, "@data", "BarrelFlowBudgetRegisterType", dataTable);
        var value = await command.ExecuteNonQueryAsync();

        return value > 0;
    }

    public async Task<List<BarrelFlowBudgetRegisterDto>> GetBarrelFlowBudgetRegister(int businessUnitId,
        DateTime dateFrom,
        DateTime dateTo)
    {
        using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"EXEC SP_BarrelFlowBudgetRegister @BusinessUnitId, @DateFrom, @DateTo";

        command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
        command.Parameters.AddWithValue(command, "@DateFrom", dateFrom);
        command.Parameters.AddWithValue(command, "@DateTo", dateTo);

        var data = new List<BarrelFlowBudgetRegisterDto>();

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            data.Add(new BarrelFlowBudgetRegisterDto
            {
                BarrelFlowBudgetRegisterId = reader.Get<int>("BarrelFlowBudgetRegisterId"),
                ProductionGross = reader.Get<decimal>("ProductionGross"),
                LossPercent = reader.Get<decimal>("LossPercent"),
                ProductionNet = reader.Get<decimal>("ProductionNet"),
                Delivery = reader.Get<decimal>("Delivery"),
                Period = reader.Get<int>("Period"),
                Date = reader.Get<DateTime>("Date"),
                BarrelFactor = reader.Get<decimal>("BarrelFactor"),
                ExportOilPercent = reader.Get<decimal>("ExportOilPercent"),
                ExportOil = reader.Get<decimal>("ExportOil"),
                COPercent = reader.Get<decimal>("COPercent"),
                CO = reader.Get<decimal>("CO"),
                CreatedBy = reader.Get<int>("CreatedBy"),
                FullName = reader.Get<string>("FullName"),
                CreatedDate = reader.Get<DateTime>("CreatedDate"),
                BusinessUnitId = reader.Get<int>("BusinessUnitId"),
                Status = reader.Get<string>("Status")
            });
        }

        return data;
    }

    public async Task<bool> SaveProductionRevenueRegisterIUD(DataTable dataTable)
    {
        try
        {
            await using var command = _unitOfWork.CreateCommand() as SqlCommand;
            command.CommandText = @"EXEC SP_ProductionRevenueRegister_IUD @data";

            command.Parameters.AddTableValue(command, "@data", "ProductionRevenueRegisterType", dataTable);
            var value = await command.ExecuteNonQueryAsync();

            return value > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<ProductionRevenueListDto>> GetProductionRevenueRegister(int businessUnitId,
        DateTime dateFrom,
        DateTime dateTo)
    {
        using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"EXEC SP_ProductionRevenueRegister @BusinessUnitId, @DateFrom, @DateTo";

        command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
        command.Parameters.AddWithValue(command, "@DateFrom", dateFrom);
        command.Parameters.AddWithValue(command, "@DateTo", dateTo);

        var data = new List<ProductionRevenueListDto>();

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            data.Add(new ProductionRevenueListDto
            {
                ProductionRevenueRegisterId = reader.Get<int>("ProductionRevenueRegisterId"),
                FactForecastId = reader.Get<int>("FactForecastId"),
                FactForecast = reader.Get<string>("FactForecast"),
                BatchNumber = reader.Get<string>("BatchNumber"),
                Status = reader.Get<int>("Status"),
                StatusName = reader.Get<string>("StatusName"),
                DeliveryQuarter = reader.Get<int>("DeliveryQuarter"),
                DeliveryMonth = reader.Get<string>("DeliveryMonth"),
                DeliveryDate = reader.Get<DateTime>("DeliveryDate"),
                DeliveredGrossTon = reader.Get<decimal>("DeliveredGrossTon"),
                DeliveredNetTon = reader.Get<decimal>("DeliveredNetTon"),
                BarrelFactor = reader.Get<decimal>("BarrelFactor"),
                SalesQuarter = reader.Get<int>("SalesQuarter"),
                SalesMonth = reader.Get<string>("SalesMonth"),
                SalesDate = reader.Get<DateTime?>("SalesDate"),
                SoldNetTon = reader.Get<decimal>("SoldNetTon"),
                SalesPrice = reader.Get<decimal>("SalesPrice"),
                SoldBarrel = reader.Get<decimal>("SoldBarrel"),
                GrossAmount = reader.Get<decimal>("GrossAmount"),
                TransportBankFee = reader.Get<decimal>("TransportBankFee"),
                MOBankfee = reader.Get<decimal>("MOBankfee"),
                CertifCustoms = reader.Get<decimal>("CertifCustoms"),
                TranportRatePerTon = reader.Get<decimal>("TranportRatePerTon"),
                AdvanceAmount = reader.Get<decimal>("AdvanceAmount"),
                AdvanceQuarter = reader.Get<int>("AdvanceQuarter"),
                AdvanceMonth = reader.Get<string>("AdvanceMonth"),
                AdvanceDate = reader.Get<DateTime?>("AdvanceDate"),
                PaymentQuarter = reader.Get<int>("PaymentQuarter"),
                PaymentMonth = reader.Get<string>("PaymentMonth"),
                FinalPaymentDate = reader.Get<DateTime?>("FinalPaymentDate"),
                UserId = reader.Get<int>("UserId"),
                BusinessUnitId = reader.Get<int>("BusinessUnitId"),
                CreatedBy = reader.Get<int>("CreatedBy"),
                FullName = reader.Get<string>("FullName"),
                CreatedDate = reader.Get<DateTime?>("CreatedDate"),
            });
        }

        return data;
    }

    public async Task<List<FactForecastListDto>> GetFactForecastList()
    {
        using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"EXEC SP_FactForecastList";

        var data = new List<FactForecastListDto>();

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            data.Add(new FactForecastListDto
            {
                FactForecastId = reader.Get<int>("FactForecastId"),
                Name = reader.Get<string>("Name"),
            });
        }

        return data;
    }

    public async Task<List<QuarterListDto>> GetQuarterList()
    {
        using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"EXEC SP_QuarterList";

        var data = new List<QuarterListDto>();

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            data.Add(new QuarterListDto
            {
                QuarterId = reader.Get<int>("QuarterId"),
                Quarter = reader.Get<string>("Quarter"),
            });
        }

        return data;
    }

    public async Task<decimal> GetBarrelFlowRegisterOpeningPeriod(int businessUnitId, int period)
    {
        using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"EXEC SP_BarrelFlowRegisterOpeningPeriod @BusinessUnitId, @Period";

        command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
        command.Parameters.AddWithValue(command, "@Period", period);

        using var reader = await command.ExecuteReaderAsync();
        decimal result = 0;
        if (reader.Read())
        {
            result = reader.Get<decimal>("ClosingOilStockTon");
        }

        return result;
    }
}