using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers;

[Microsoft.AspNetCore.Components.Route("api/[controller]")]
[Authorize]
public class OrderController : CustomBaseController
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpGet("{businessUnitId}")]
    public async Task<IActionResult> Type(int businessUnitId)
        => CreateActionResult(await _orderService.GetTypesAsync(businessUnitId));
}