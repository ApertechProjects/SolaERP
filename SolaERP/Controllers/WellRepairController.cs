using SolaERP.Application.Dtos.WellRepair;

namespace SolaERP.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Contracts.Services;
using SolaERP.Controllers;

[Route("api/[controller]")]
[Authorize]
public class WellRepairController : CustomBaseController
{
    private readonly IWellRepairService _wellRepairService;
    
    public WellRepairController(IWellRepairService wellRepairService) => 
        _wellRepairService = wellRepairService;

    [HttpGet("[action]")]
    public async Task<IActionResult> GetWellRepairs()
    {
        return CreateActionResult(
            await _wellRepairService.GetWellRepairList());
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> LoadWellRepairs(int wellRepairId)
    {
        return CreateActionResult(
            await _wellRepairService.LoadWellRepairs(wellRepairId));
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetWellCostList (int businessUnitId, DateTime dateFrom,  DateTime dateTo)
    {
        return CreateActionResult(
            await _wellRepairService.GetWellCostList(businessUnitId, dateFrom, dateTo));
    }
    
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetAnalysisListFromSun (int businessUnitId, int anlCatId)
    {
        return CreateActionResult(
            await _wellRepairService.GetAnalysisListFromSun(businessUnitId, anlCatId));
    }
    
    
    [HttpPost("[action]")]
    public async Task<IActionResult> SaveWellRepair(List<WellRepairRequest> data)
    {
        return CreateActionResult(await _wellRepairService.SaveWellRepairAsync(data,  Convert.ToInt32(User.Identity.Name)));
    }   
    
    [HttpPost("[action]")]
    public async Task<IActionResult> SaveWellCost(List<WellCostRequest> data)
    {
        return CreateActionResult(await _wellRepairService.SaveWellCostAsync(data,  Convert.ToInt32(User.Identity.Name)));
    }
    
}
































