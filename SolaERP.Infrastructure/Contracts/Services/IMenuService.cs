using SolaERP.Infrastructure.Dtos.Menu;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.User;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IMenuService
    {
        Task<ApiResponse<List<ParentMenuDto>>> GetUserMenusWithChildrenAsync(string finderToken);
        Task<ApiResponse<List<MenuWithPrivilagesDto>>> GetUserMenusWithPrivilegesAsync(string finderToken);
        Task<ApiResponse<GroupMenuResponseDto>> GetGroupMenuWithPrivilegeListByGroupIdAsync(string finderToken, int groupId);
        Task<ApiResponse<AdditionalPrivilegeAccessDto>> GetAdditionalPrivilegeAccessAsync(string authToken);
    }
}
