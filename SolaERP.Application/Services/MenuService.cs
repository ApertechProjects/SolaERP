using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Menu;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Application.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public MenuService(IMenuRepository menuRepository, IUserRepository userRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<ParentMenuDto>>> GetUserMenusWithChildsAsync(string finderToken)
        {
            var menusWithPrivilages = await _menuRepository.GetUserMenuWithPrivillagesAsync(
                await _userRepository.GetUserIdByTokenAsync(finderToken));

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
        public async Task<ApiResponse<List<MenuWithPrivilagesDto>>> GetUserMenusWithPrivilagesAsync(string finderToken)
        {
            var menus = await _menuRepository.GetUserMenuWithPrivillagesAsync(await _userRepository.GetUserIdByTokenAsync(finderToken));
            var menusDto = _mapper.Map<List<MenuWithPrivilagesDto>>(menus);

            if (menusDto != null)
                return ApiResponse<List<MenuWithPrivilagesDto>>.Success(menusDto, 200);

            return ApiResponse<List<MenuWithPrivilagesDto>>.Fail("BadRequest", 400);
        }
    }
}
