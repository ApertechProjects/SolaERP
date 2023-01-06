using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Menu;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public async Task<ApiResponse<List<ParentMenuDto>>> GetUserMenusWithChilds([FromHeader] string authToken)
        {
            return await _menuService.GetUserMenusWithChildsAsync(authToken);
        }

        [HttpGet]
        public async Task<ApiResponse<List<MenuWithPrivilagesDto>>> GetUserMenusWithPrivilages([FromHeader] string authToken)
        {
            return await _menuService.GetUserMenusWithPrivilagesAsync(authToken);
        }

        [HttpGet]
        public async Task<ApiResponse<GroupMenuResponseDto>> GetGroupMenuWithPrivillageList([FromHeader] string authToken, int groupId)
        {
            return await _menuService.GetGroupMenuWithPrivillageListByGroupIdAsync(authToken, groupId);
        }
    }
}
