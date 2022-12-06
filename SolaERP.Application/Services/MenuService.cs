using SolaERP.Application.Utils;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Menu;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Application.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;

        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<ApiResponse<List<ParentMenuDto>>> GetUserMenusWithChildsAsync()
        {
            var menusWithPrivilages = await _menuRepository.GetUserMenuWithPrivillagesAsync(Kernel.CurrentUserId);
            var parentMenusWithPrivilages = menusWithPrivilages.Where(m => m.ParentId == 0).ToList();//For getting ParentMenus

            List<ParentMenuDto> menus = new List<ParentMenuDto>();
            foreach (var parent in parentMenusWithPrivilages)
            {
                ParentMenuDto parentMenu = new ParentMenuDto()
                {
                    Id = parent.MenuId,
                    ParentMenuName = parent.MenuName,
                    Icon = parent.Icon,
                    ReactIcon = parent.ReactIcon,
                    Url = parent.Url
                };

                var childMenus = menusWithPrivilages.Where(m => m.ParentId == parent.MenuId).ToList();

                foreach (var child in childMenus)
                {
                    parentMenu.Childs.Add(new()
                    {
                        Id = child.MenuId,
                        ParentMenuId = child.ParentId,
                        Icon = child.Icon,
                        Url = child.Url,
                        ReactIcon = child.ReactIcon,
                        ChildMenuName = child.MenuName,
                        ParentMenu = parentMenu
                    });
                }
                menus.Add(parentMenu);
            }
            if (menus.Count > 0)
                return ApiResponse<List<ParentMenuDto>>.Success(menus, 200);

            return ApiResponse<List<ParentMenuDto>>.Fail("BadRequest", 400);
        }
    }
}
