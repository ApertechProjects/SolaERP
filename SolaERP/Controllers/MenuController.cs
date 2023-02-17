using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class MenuController : CustomBaseController
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserMenusWithChilds([FromHeader] string authToken)
            => CreateActionResult(await _menuService.GetUserMenusWithChildsAsync(authToken));

        [HttpGet]
        public async Task<IActionResult> GetUserMenusWithPrivilages([FromHeader] string authToken)
            => CreateActionResult(await _menuService.GetUserMenusWithPrivilagesAsync(authToken));

        [HttpGet]
        public async Task<IActionResult> GetGroupMenuWithPrivillageList([FromHeader] string authToken, int groupId)
            => CreateActionResult(await _menuService.GetGroupMenuWithPrivillageListByGroupIdAsync(authToken, groupId));

        [HttpGet]
        public async Task<IActionResult> GetAdditionalPrivilegeAccessAsync(string authToken)
        => CreateActionResult(await _menuService.GetAdditionalPrivilegeAccessAsync(authToken));
    }
}
