using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Menu;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.User;
using System.Xml.Linq;

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

        public async Task<ApiResponse<List<ParentMenuDto>>> GetUserMenusWithChildrenAsync(string name)
        {
            var menusWithPrivilages = await _menuRepository.GetUserMenuWithPrivilegesAsync(
                await _userRepository.GetIdentityNameAsIntAsync(name));

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
                    Url = parent.Url,
                    ReadAccess = parent.ReadAccess,
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
                        ParentMenu = parentMenu,
                        ReadAccess = child.ReadAccess
                    });
                }
                menus.Add(parentMenu);
            }
            if (menus.Count > 0)
                return ApiResponse<List<ParentMenuDto>>.Success(menus, 200);

            return ApiResponse<List<ParentMenuDto>>.Fail("This user doesn't have any privileges in our system", 400);
        }
        public async Task<ApiResponse<List<MenuWithPrivilagesDto>>> GetUserMenusWithPrivilegesAsync(string name)
        {
            var menus = await _menuRepository.GetUserMenuWithPrivilegesAsync(await _userRepository.GetIdentityNameAsIntAsync(name));
            var menusDto = _mapper.Map<List<MenuWithPrivilagesDto>>(menus);

            if (menusDto != null)
                return ApiResponse<List<MenuWithPrivilagesDto>>.Success(menusDto, 200);

            return ApiResponse<List<MenuWithPrivilagesDto>>.Fail("This user doesn't have any privileges in our system", 400);
        }
        public async Task<ApiResponse<GroupMenuResponseDto>> GetGroupMenuWithPrivilegeListByGroupIdAsync(string name, int groupId)
        {
            var userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            var menus = await _menuRepository.GetUserMenuWithPrivilegesAsync(userId);

            GroupMenuResponseDto response = new() { Menus = _mapper.Map<List<MenuWithPrivilagesDto>>(menus) };
            GroupMenuWithPrivillageIdListDto groupMenusWithPrivList = new();

            if (groupId > 0)
            {
                var groupMenus = await _menuRepository.GetGroupMenusByGroupIdAsync(groupId);
                foreach (var userMenu in menus)
                {
                    var comparer = groupMenus.Where(gm => gm.MenuId == userMenu.MenuId).FirstOrDefault();
                    if (comparer != null)
                    {
                        if (comparer.CreateAccess == 1)
                            groupMenusWithPrivList.Create.Add(comparer.MenuId);
                        if (comparer.DeleteAccess == 1)
                            groupMenusWithPrivList.Delete.Add(comparer.MenuId);
                        if (comparer.ExportAccess == 1)
                            groupMenusWithPrivList.Export.Add(comparer.MenuId);
                        if (comparer.EditAccess == 1)
                            groupMenusWithPrivList.Edit.Add(comparer.MenuId);
                    }
                }
            }
            response.PrivillageList = groupMenusWithPrivList;
            return ApiResponse<GroupMenuResponseDto>.Success(response, 200);
        }

        public async Task<ApiResponse<AdditionalPrivilegeAccessDto>> GetAdditionalPrivilegeAccessAsync(string name)
        {
            int userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            var users = await _menuRepository.GetAdditionalPrivilegeAccessAsync(userId);
            var dto = _mapper.Map<AdditionalPrivilegeAccessDto>(users);
            return ApiResponse<AdditionalPrivilegeAccessDto>.Success(dto, 200);
        }
    }
}
