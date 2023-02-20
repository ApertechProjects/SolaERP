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

        [HttpPost]
        public async Task<IActionResult> GetItemCodesByItemCodeAsync(string itemCode)
            => CreateActionResult(await _itemService.GetItemCodeByItemCodeAsync(itemCode));

        [HttpGet]
        public async Task<IActionResult> GetItemCodesTestAsync([FromHeader] string authToken)
        {
            List<ExecuteQueryParamList> executeQueryParamLists = new List<ExecuteQueryParamList>();
            executeQueryParamLists.Add(new ExecuteQueryParamList { ParamName = "APT", Value = "APT" });

            List<ExecuteQueryParamList> executeQueryParamLists2 = new List<ExecuteQueryParamList>();
            executeQueryParamLists2.Add(new ExecuteQueryParamList { ParamName = "@ItemCode", Value = "01070201001" });
            return CreateActionResult(await _itemService.ExecQueryWithReplace("[dbo].[GET_ITEM_BY_ITEM_CODE]"
                                                                            , executeQueryParamLists, executeQueryParamLists2));
        }

    }
}
