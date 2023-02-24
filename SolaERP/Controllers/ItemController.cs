using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ItemController : CustomBaseController
    {
        private readonly IItemService _itemService;
        public ItemController(IItemService itemService)
        {
            this._itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetItemCodesAsync()
            => CreateActionResult(await _itemService.GetAllAsync());

        [HttpGet("{itemCode}")]
        public async Task<IActionResult> GetItemCodesByItemCodeAsync(string itemCode)
            => CreateActionResult(await _itemService.GetItemCodeByItemCodeAsync(itemCode));

        [HttpGet("{itemCode}")]
        public async Task<IActionResult> GetItemCodeInfoByItemCodeAsync(string itemCode)
            => CreateActionResult(await _itemService.GetItemCodeInfoByItemCodeAsync(itemCode));


    }
}
