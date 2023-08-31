using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Order;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers;

[Route("api/[controller]")]
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
    public async Task<IActionResult> GetWFA([FromBody] OrderWFAFilterDto filterDto)
        => CreateActionResult(await _orderService.GetWFAAsync(filterDto, User.Identity.Name));

    [HttpPost("[action]")]
    public async Task<IActionResult> GetChangeApproval([FromBody] OrderChangeApprovalFilterDto filterDto)
        => CreateActionResult(await _orderService.GetChangeApprovalAsync(filterDto, User.Identity.Name));

    [HttpPost("[action]")]
    public async Task<IActionResult> GetHeld([FromBody] OrderHeldFilterDto filter)
        => CreateActionResult(await _orderService.GetHeldAsync(filter, User.Identity.Name));

    [HttpPost("[action]")]
    public async Task<IActionResult> GetRejected([FromBody] OrderRejectedFilterDto filter)
        => CreateActionResult(await _orderService.GetRejectedAsync(filter, User.Identity.Name));

    [HttpPost("[action]")]
    public async Task<IActionResult> GetDraft([FromBody] OrderDraftFilterDto filter)
        => CreateActionResult(await _orderService.GetDraftAsync(filter, User.Identity.Name));

    [HttpPost("[action]")]
    public async Task<IActionResult> Add([FromBody] OrderMainDto orderMainDto)
        => CreateActionResult(await _orderService.AddAsync(orderMainDto, User.Identity.Name));

    [HttpDelete("{orderMainId}")]
    public async Task<IActionResult> Delete(int orderMainId)
        => CreateActionResult(await _orderService.DeleteAsync(orderMainId, User.Identity.Name));
}