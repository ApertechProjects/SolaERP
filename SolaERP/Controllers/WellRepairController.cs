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
}