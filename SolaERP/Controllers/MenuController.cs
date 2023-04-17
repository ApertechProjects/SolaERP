using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
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
        public async Task<IActionResult> GetUserMenusWithChildren()
            => CreateActionResult(await _menuService.GetUserMenusWithChildrenAsync(User.Identity.Name));

        [HttpGet]
        public async Task<IActionResult> GetUserMenusWithPrivileges()
            => CreateActionResult(await _menuService.GetUserMenusWithPrivilegesAsync(User.Identity.Name));

        [HttpGet]
        public async Task<IActionResult> GetGroupMenuWithPrivilegeList(int groupId)
            => CreateActionResult(await _menuService.GetGroupMenuWithPrivilegeListByGroupIdAsync(User.Identity.Name, groupId));

        [HttpGet]
        public async Task<IActionResult> GetAdditionalPrivilegeAccessAsync()
        => CreateActionResult(await _menuService.GetAdditionalPrivilegeAccessAsync(User.Identity.Name));
    }
}
