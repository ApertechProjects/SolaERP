using SolaERP.Infrastructure.Dtos.Menu;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.User;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IMenuService
    {
        Task<ApiResponse<List<ParentMenuDto>>> GetUserMenusWithChildrenAsync(string name);
        Task<ApiResponse<List<MenuWithPrivilagesDto>>> GetUserMenusWithPrivilegesAsync(string name);
        Task<ApiResponse<GroupMenuResponseDto>> GetGroupMenuWithPrivilegeListByGroupIdAsync(string name, int groupId);
        Task<ApiResponse<AdditionalPrivilegeAccessDto>> GetAdditionalPrivilegeAccessAsync(string name);
    }
}
