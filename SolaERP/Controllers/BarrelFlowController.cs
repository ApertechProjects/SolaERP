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
        return CreateActionResult(await _barrelFlowService.BarrelFlowRegisterAsync(businessUnitId, dateFrom, dateTo));
    }
}