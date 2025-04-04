using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers;

[Route("api/[controller]")]
[Authorize]
public class FixedAssetController : CustomBaseController
{
    private readonly IFixedAssetService _fixedAssetService;

    public FixedAssetController(IFixedAssetService fixedAssetService)
    {
        _fixedAssetService = fixedAssetService;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll([FromQuery] int businessUnitId)
        => CreateActionResult(await _fixedAssetService.GetAllAsync(businessUnitId));
}