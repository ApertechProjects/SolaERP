using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.BarrelFlow;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers;

[Route("api/[controller]")]
[Authorize]
public class BarrelFlowController : CustomBaseController
{
    private readonly IBarrelFlowService _barrelFlowService;

    public BarrelFlowController(IBarrelFlowService barrelFlowService) =>
        _barrelFlowService = barrelFlowService;

    [HttpPost("[action]")]
    public async Task<IActionResult> SaveBarrelFlows(List<BarrelFlowUIDDto> data)
    {
        return CreateActionResult(await _barrelFlowService.SaveBarrelFlowsAsync(data));
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> BarrelFlowRegister(int businessUnitId, DateTime dateFrom, DateTime dateTo)
    {
        return CreateActionResult(
            await _barrelFlowService.GetBarrelFlowRegisterAsync(businessUnitId, dateFrom, dateTo));
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> PeriodList(int businessUnitId)
    {
        return CreateActionResult(await _barrelFlowService.GetPeriodListByBusinessIdAsync(businessUnitId));
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> SaveBarrelFlowBudget(List<BarrelFlowBudgetUIDDto> data)
    {
        return CreateActionResult(await _barrelFlowService.SaveBarrelFlowBudgetRegisterAsync(data));
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> BarrelFlowBudgetRegister(int businessUnitId, DateTime dateFrom, DateTime dateTo)
    {
        return CreateActionResult(
            await _barrelFlowService.GetBarrelFlowBudgetRegisterAsync(businessUnitId, dateFrom, dateTo));
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> SaveProductionRevenue(List<ProductionRevenueRegisterIUDDto> data)
    {
        return CreateActionResult(await _barrelFlowService.SaveProductionRevenueRegisterAsync(data));
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> ProductionRevenueRegister(int businessUnitId, DateTime dateFrom, DateTime dateTo)
    {
        return CreateActionResult(
            await _barrelFlowService.GetProductionRevenueRegisterAsync(businessUnitId, dateFrom, dateTo));
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> FactForecastList()
    {
        return CreateActionResult(
            await _barrelFlowService.GetFactForecastListAsync());
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> QuarterList()
    {
        return CreateActionResult(
            await _barrelFlowService.GetQuarterListAsync());
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> BarrelFlowRegisterOpeningPeriod(int businessUnitId,
        int period)
    {
        return CreateActionResult(
            await _barrelFlowService.GetBarrelFlowRegisterOpeningPeriodAsync(businessUnitId, period));
    }
}