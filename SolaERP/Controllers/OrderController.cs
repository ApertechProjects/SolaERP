using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Order;
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

    [HttpPost("[action]")]
    public async Task<IActionResult> GetAll([FromBody] OrderFilterDto filter)
        => CreateActionResult(await _orderService.GetAllAsync(filter, User.Identity.Name));

    [HttpPost("[action]")]
    public async Task<IActionResult> GetChangeApproval([FromBody] OrderChangeApprovalFilterDto filterDto)
        => CreateActionResult(await _orderService.GetChangeApprovalAsync(filterDto, User.Identity.Name));

    [HttpPost("[action]")]
    public async Task<IActionResult> GetHeld([FromBody] OrderFilterDto filter)
        => CreateActionResult(await _orderService.GetAllAsync(filter, User.Identity.Name));

    [HttpPost("[action]")]
    public async Task<IActionResult> GetRejected([FromBody] OrderFilterDto filter)
        => CreateActionResult(await _orderService.GetAllAsync(filter, User.Identity.Name));

    [HttpPost("[action]")]
    public async Task<IActionResult> GetDraft([FromBody] OrderFilterDto filter)
        => CreateActionResult(await _orderService.GetAllAsync(filter, User.Identity.Name));
}