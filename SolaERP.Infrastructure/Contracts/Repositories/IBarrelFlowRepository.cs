using System.Data;
using SolaERP.Application.Dtos.BarrelFlow;

namespace SolaERP.Application.Contracts.Repositories;

public interface IBarrelFlowRepository
{
    Task<bool> SaveBarrelFlowsRegisterIUD(DataTable dataTable);
    Task<List<BarrelFlowRegisterDto>> GetBarrelFlowRegister(int businessUnitId, DateTime dateFrom, DateTime dateTo);
    Task<List<BarrelFlowPeriodListDto>> GetPeriodListByBusinessId(int businessUnitId);
    Task<bool> SaveBarrelFlowBudgetRegisterIUD(DataTable dataTable);

    Task<List<BarrelFlowBudgetRegisterDto>> GetBarrelFlowBudgetRegister(int businessUnitId, DateTime dateFrom,
        DateTime dateTo);

    Task<bool> SaveProductionRevenueRegisterIUD(DataTable dataTable);

    Task<List<ProductionRevenueListDto>> GetProductionRevenueRegister(int businessUnitId, DateTime dateFrom,
        DateTime dateTo);

    Task<List<FactForecastListDto>> GetFactForecastList();
    Task<List<QuarterListDto>> GetQuarterList();
    Task<decimal> GetBarrelFlowRegisterOpeningPeriod(int businessUnitId, int period);
}