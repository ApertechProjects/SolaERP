using SolaERP.Application.Dtos.BarrelFlow;
using SolaERP.Application.Dtos.BidComparison;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Contracts.Services;

public interface IBarrelFlowService
{
    Task<ApiResponse<bool>> SaveBarrelFlowsAsync(List<BarrelFlowUIDDto> data);

    Task<ApiResponse<List<BarrelFlowRegisterDto>>> GetBarrelFlowRegisterAsync(int businessUnitId,
        DateTime dateFrom, DateTime dateTo);

    Task<ApiResponse<List<BarrelFlowPeriodConvertListDto>>> GetPeriodListByBusinessIdAsync(int businessUnitId);
    Task<ApiResponse<bool>> SaveBarrelFlowBudgetRegisterAsync(List<BarrelFlowBudgetUIDDto> data);

    Task<ApiResponse<List<BarrelFlowBudgetRegisterDto>>> GetBarrelFlowBudgetRegisterAsync(int businessUnitId,
        DateTime dateFrom, DateTime dateTo);

    Task<ApiResponse<bool>> SaveProductionRevenueRegisterAsync(List<ProductionRevenueRegisterIUDDto> data);

    Task<ApiResponse<List<ProductionRevenueListDto>>> GetProductionRevenueRegisterAsync(int businessUnitId,
        DateTime dateFrom, DateTime dateTo);

    Task<ApiResponse<List<FactForecastListDto>>> GetFactForecastListAsync();
    Task<ApiResponse<List<QuarterListDto>>> GetQuarterListAsync();

    Task<ApiResponse<List<BarrelFlowRegisterOpeningPeriodDto>>> GetBarrelFlowRegisterOpeningPeriodAsync(int businessUnitId, int period);

    Task<ApiResponse<List<BarrelFlowPeriodListDto>>> GetPeriodListByBusinessId(int businessUnitId);
}