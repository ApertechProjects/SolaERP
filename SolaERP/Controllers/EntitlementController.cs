using SolaERP.Application.Dtos.Entitlement;

namespace SolaERP.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Controllers;

[Route("api/[controller]")]
[Authorize]
public class EntitlementController : CustomBaseController
{
    private readonly IEntitlementService _entitlementService;

    public EntitlementController(IEntitlementService entitlementService) =>
        _entitlementService = entitlementService;

    [HttpPost("[action]")]
    public async Task<IActionResult> SaveEntitlements(EntitlementUIDDto data)
    {
        return CreateActionResult(await _entitlementService.SaveEntitlementsAsync(data));
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetEntitlementsList(int businessUnitId, int periodFrom, int periodTo)
    {
        return CreateActionResult(
            await _entitlementService.GetEntitlementListAsync(businessUnitId, periodFrom, periodTo));
    }
}