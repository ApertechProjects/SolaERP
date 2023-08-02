using SolaERP.Application.Dtos.Menu;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.User;

namespace SolaERP.Application.Contracts.Services
{
    public interface IMenuService
    {
        Task<ApiResponse<List<ParentMenuDto>>> GetUserMenusWithChildrenAsync(string name);
        Task<ApiResponse<List<MenuWithPrivilagesDto>>> GetUserMenusWithPrivilegesAsync(string name);
        Task<ApiResponse<GroupMenuResponseDto>> GetGroupMenuWithPrivilegeListByGroupIdAsync(string name, int groupId);
        Task<ApiResponse<List<MenuWithPrivilege>>> GetMenuWithPrivilegeListByGroupIdAsync(int groupId);
        Task<ApiResponse<AdditionalPrivilegeAccessDto>> GetAdditionalPrivilegeAccessAsync(string name);
    }
}
