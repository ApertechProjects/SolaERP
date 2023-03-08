using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [Authorize]
    public class MenuController : CustomBaseController
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        /// <summary>
        /// Gets the list of users.
        /// </summary>
        /// <remarks>
        /// This endpoint returns the list of all users in the system.
        /// </remarks>
        /// <returns>The list of users.</returns>
        [HttpGet]
        public async Task<IActionResult> GetUserMenusWithChildren([FromHeader] string authToken)
            => CreateActionResult(await _menuService.GetUserMenusWithChildrenAsync(authToken));

        [HttpGet]
        public async Task<IActionResult> GetUserMenusWithPrivileges([FromHeader] string authToken)
            => CreateActionResult(await _menuService.GetUserMenusWithPrivilegesAsync(authToken));

        [HttpGet]
        public async Task<IActionResult> GetGroupMenuWithPrivilegeList([FromHeader] string authToken, int groupId)
            => CreateActionResult(await _menuService.GetGroupMenuWithPrivilegeListByGroupIdAsync(authToken, groupId));

        [HttpGet]
        public async Task<IActionResult> GetAdditionalPrivilegeAccessAsync(string authToken)
        => CreateActionResult(await _menuService.GetAdditionalPrivilegeAccessAsync(authToken));
    }
}
