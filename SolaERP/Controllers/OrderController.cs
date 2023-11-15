using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Order;
using SolaERP.Application.Dtos.Vendors;
using SolaERP.Application.Models;
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

    [HttpGet("[action]/{businessUnitId}")]
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
    public async Task<IActionResult> Save([FromBody] OrderMainDto orderMainDto)
        => CreateActionResult(await _orderService.AddAsync(orderMainDto, User.Identity.Name));

    [HttpPost("[action]")]
    public async Task<IActionResult> ChangeOrderMainStatus([FromBody] ChangeOrderMainStatusDto statusDto)
        => CreateActionResult(await _orderService.ChangeOrderMainStatusAsync(statusDto, User.Identity.Name));

    [HttpPost("[action]")]
    public async Task<IActionResult> Delete([FromBody] List<int> orderMainIdList)
        => CreateActionResult(await _orderService.DeleteAsync(orderMainIdList, User.Identity.Name));

    [HttpPost("[action]")]
    public async Task<IActionResult> SendToApprove([FromBody] List<int> orderMainIdList)
        => CreateActionResult(await _orderService.SendToApproveAsync(orderMainIdList, User.Identity.Name));

    [HttpGet("[action]/{orderMainId}")]
    public async Task<IActionResult> GetHeaderLoad(int orderMainId)
        => CreateActionResult(await _orderService.GetHeaderLoadAsync(orderMainId));

    [HttpPost("[action]")]
    public async Task<IActionResult> GetOrderCreateListForRequest([FromBody] OrderCreateListRequest dto)
        => CreateActionResult(await _orderService.GetOrderCreateListForRequestAsync(dto));

    [HttpPost("[action]")]
    public async Task<IActionResult> GetOrderCreateListForBids([FromBody] OrderCreateListRequest dto)
        => CreateActionResult(await _orderService.GetOrderCreateListForBidsAsync(dto));

    [HttpGet("[action]")]
    public async Task<IActionResult> GetOrderCard() => CreateActionResult(await _orderService.GetOrderCardAsync());

    [HttpGet("[action]/{vendorId}")]
    public async Task<IActionResult> GetWithHoldingTaxData(int vendorId) =>
        CreateActionResult(await _orderService.WithHoldingTaxDatas(vendorId));

    [HttpPost("[action]")]
    public async Task<IActionResult> Retrieve(OrderRetrieveModel ids)
        => CreateActionResult(await _orderService.Retrieve(ids.ids, User.Identity.Name));

}