using AutoMapper;
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
        private readonly IMapper _mapper;

        public MenuService(IMenuRepository menuRepository,
                            IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<ParentMenuDto>>> GetUserMenusWithChildsAsync()
        {
            var menusWithPrivvilages = await _menuRepository.GetUserMenuWithPrivillagesAsync(Kernel.CurrentUserId);
            var menuDto = _mapper.Map<List<MenuWithPrivilagesDto>>(menusWithPrivvilages);
            var parentMenusWithPrivilages = menuDto.Where(m => m.ParentId == 0).ToList();

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

                var childMenus = menuDto.Where(m => m.ParentId == parent.MenuId).ToList();

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
            return ApiResponse<List<ParentMenuDto>>.Success(menus, 200);
        }
    }
}
