using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;

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


        [HttpGet]
        public async Task<IActionResult> GetUserMenusWithPrivileges()
            => CreateActionResult(await _menuService.GetUserMenusWithPrivilegesAsync(User.Identity.Name));


        [HttpGet]
        public async Task<IActionResult> GetAdditionalPrivilegeAccessAsync()
        => CreateActionResult(await _menuService.GetAdditionalPrivilegeAccessAsync(User.Identity.Name));
    }
}
