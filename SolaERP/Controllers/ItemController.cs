using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ItemController : CustomBaseController
    {
        private readonly IItemService _itemService;
        public ItemController(IItemService itemService)
        {
            this._itemService = itemService;
        }

        [HttpGet("{businessUnitCode}")]
        public async Task<IActionResult> GetItemCodesAsync(string businessUnitCode)
            => CreateActionResult(await _itemService.GetAllAsync(businessUnitCode));

        [HttpGet("{businessUnitCode}/{itemCode}")]
        public async Task<IActionResult> GetItemCodesByItemCodeAsync(string businessUnitCode, string itemCode)
            => CreateActionResult(await _itemService.GetItemCodeByItemCodeAsync(businessUnitCode, itemCode));

        [HttpGet]
        public async Task<IActionResult> GetItemCodeInfoByItemCodeAsync(string itemCode, int businessUnitId)
            => CreateActionResult(await _itemService.GetItemCodeInfoByItemCodeAsync(itemCode, businessUnitId));


    }
}
